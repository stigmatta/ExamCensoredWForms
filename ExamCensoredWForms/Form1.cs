
using System.Runtime.CompilerServices;

namespace ExamCensoredWForms
{
    public partial class Form1 : Form
    {
        private List<string> words = new List<string>();
        private CancellationTokenSource tokenSource;
        private SynchronizationContext sc;
        private string foundFilesDir = "foundFiles";

        public Form1()
        {
            InitializeComponent();
            InitializeListView();
            sc = SynchronizationContext.Current;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void InitializeListView()
        {
            listView1.View = View.Details;
            listView1.Columns.Add("Path", 100);
            listView1.Columns.Add("Size", 80);
            listView1.Columns.Add("Words",200);
        }

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 && openFileDialog1.FileName == string.Empty)
            {
                MessageBox.Show("No text entered and no file selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (folderBrowserAnalyze.SelectedPath == string.Empty)
            {
                MessageBox.Show("No folder to analyze selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            buttonStart.Enabled = false;
            buttonEnd.Enabled = true;
            tokenSource = new CancellationTokenSource();

            try
            {
                string str = await File.ReadAllTextAsync(openFileDialog1.FileName, tokenSource.Token);
                if (str == string.Empty)
                {
                    MessageBox.Show("Input file is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SplitWordsBySpace(str);



                await LoadProgressBar(30, tokenSource.Token);
                CensoreAllToFile();

                await SearchWordsInFilesAsync(words, tokenSource.Token);

                ReportAllToFile(tokenSource.Token);


            }
            catch (OperationCanceledException)
            {
                progressBar1.Value = 0;
                MessageBox.Show("Operation was canceled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void SplitWordsBySpace(string allText)
        {
            words = allText.Trim().Split(' ').ToList();
        }

        private async void CensoreAllToFile()
        {
            string tmp = string.Join(" ", words.Select(word => "*******"));
            await File.WriteAllTextAsync("censoredCopy.txt", tmp, tokenSource.Token);
            await LoadProgressBar(10, tokenSource.Token);
        }

        private async void ReportAllToFile(CancellationToken token)
        {
            string filePath = Path.Combine(Application.StartupPath, "Report.txt");

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    token.ThrowIfCancellationRequested();
                    await writer.WriteLineAsync("Path\tSize\tWords");

                    foreach (ListViewItem item in listView1.Items)
                    {
                        string line = string.Join("\t", item.SubItems.Cast<ListViewItem.ListViewSubItem>().Select(subItem => subItem.Text));
                        await writer.WriteLineAsync(line);
                        token.ThrowIfCancellationRequested();
                    }
                }

            }
            catch(OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            await LoadProgressBar(30, tokenSource.Token);
        }



        private async Task LoadProgressBar(int value, CancellationToken token)
        {
            int res = progressBar1.Value + value;
            for (int i = 0; i < value; i++)
            {
                if (progressBar1.Value == res)
                    return;

                sc.Send(d => progressBar1.Value++, null);

                await Task.Delay(40, token);

                token.ThrowIfCancellationRequested();
            }
        }

        private async Task SearchWordsInFilesAsync(List<string> words, CancellationToken token)
        {
            listView1.Items.Clear();
            string rootDirectory = folderBrowserAnalyze.SelectedPath;

            try
            {
                await foreach (var file in GetFilesSafeAsyncStream(rootDirectory, "*.txt", token))
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();

                        var fileContent = await File.ReadAllTextAsync(file, token);

                        var wordCounts = new Dictionary<string, int>();
                        foreach (string word in words)
                        {
                            int count = CountWordOccurrences(fileContent, word);
                            if (count > 0)
                                wordCounts[word] = count;
                        }

                        if (wordCounts.Count > 0)
                        {
                            string wordSummary = string.Join(", ", wordCounts.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
                            var item = new ListViewItem(new[] { file, new FileInfo(file).Length.ToString(), wordSummary });
                            sc.Send(_ => listView1.Items.Add(item), null);

                            await CopyFileToDirAsync(file, foundFilesDir);
                        }

                        await Task.Delay(1, token);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        continue;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }

            await LoadProgressBar(30, token);
        }


        private int CountWordOccurrences(string text, string word)
        {
            int count = 0;
            int index = 0;
            while ((index = text.IndexOf(word, index, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                count++;
                index += word.Length;
            }
            return count;
        }

        private async IAsyncEnumerable<string> GetFilesSafeAsyncStream(string directory, string searchPattern, [EnumeratorCancellation] CancellationToken token)
        {
            IEnumerable<string> files;
            try
            {
                files = Directory.EnumerateFiles(directory, searchPattern);
            }
            catch (UnauthorizedAccessException)
            {
                yield break;
            }
            catch (DirectoryNotFoundException)
            {
                yield break;
            }

            foreach (var file in files)
            {
                token.ThrowIfCancellationRequested();
                yield return file;
            }

            IEnumerable<string> subdirectories;
            try
            {
                subdirectories = Directory.EnumerateDirectories(directory);
            }
            catch (UnauthorizedAccessException)
            {
                yield break;
            }
            catch (DirectoryNotFoundException)
            {
                yield break;
            }

            foreach (var subDirectory in subdirectories)
            {
                token.ThrowIfCancellationRequested();

                await foreach (var file in GetFilesSafeAsyncStream(subDirectory, searchPattern, token))
                {
                    yield return file;
                }
            }
        }

        private async Task CopyFileToDirAsync(string filePath, string dirPath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("Исходный файл не найден", filePath);

                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                string destinationPath = Path.Combine(dirPath, Path.GetFileName(filePath));

                await Task.Run(() => File.Copy(filePath, destinationPath, overwrite: true));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка копирования файла: {ex.Message}");
            }
        }

        private void buttonEnd_Click(object sender, EventArgs e)
        {
            buttonEnd.Enabled = false;
            buttonStart.Enabled = true;

            tokenSource?.Cancel();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            folderBrowserAnalyze.ShowDialog();
        }

    }
}
