using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using Iveonik.Stemmers;
using TextFinder.Partitions;
using TextFinder.Core;
using TextFinder.Core.hindawi;



namespace TextFinder
{

    public partial class MainWindow : Window
    {

        _TypesCheck typesCheck;
        char[] delimeters;
        _DataTable dt;
        List<string> badWords = new List<string>();
        Dictionary<string, bool[]> sent = new Dictionary<string, bool[]>();
        Dictionary<string, int> dic = new Dictionary<string, int>();

        public static string[][] Keys = { 
		        /*1. Цифровой идентификатор объекта*/
		        new string[] { "DOI" },
		        /* 2. Классификаторы статьи*/
		        new string[] { "UDC", "PACS", "MSC" },
		        /*3. Название статьи */
		        new string[] { "Article Title" },
		        /*4. Журнал*/
		        new string[] { "Journal Title ", "Vol.", "Issue", "ISSN" },
		        /*5. Приоритет публикации*/
		        new string[] { "Article history", "Received", "Revised", "Final form", "Accepted", "Published", "Article published online"},
		        /*6. Контактные данные*/
		        new string[] { "Author details", "e-mail", "Correspondence" , "Tel.", "Fax"},
		        /*7. Копирайт*/
		        new string[] { "All rights reserved", "Reprints and permissions", "Licensee", "©", "Full terms and conditions of use" },
		        /*8. Издатель*/
		        new string[] { "Published by", "Printed in", "Production and hosting by" },
		        /*9. Академический редактор*/
		        new string[] { "Academic Editor", "Technical Editor"},
		        /*10. Аннотация*/
		        new string[] { "Abstract", "Аннотация" },
		        /*11. Ключевые слова*/
		        new string[] { "Keywords", "Ключевые слова" }, 
		        /*12. Условные обозначения*/
		        new string[] { "Nomenclature", "Abbreviations" },
		        /*13. Введение*/
		        new string[] { "Introduction", "Background", "Basic definitions", "Введение" },
		        /*14.  Литературный обзор*/
		        new string[] { "Existing approaches and factors", "A current state-of-the-art", "Some comments", "Some characteristics" },
		        /*15. Цели и задачи работы*/
		        new string[] { "Object of the analysis", "Objective", "Purpose" },
		        /*16. Методы исследования*/
		        new string[] { "Material", "Methods", "Research Technique", "Solution methodology" },
		        /*17. Постановка задачи*/
		        new string[] { "Problem statement", "Implemented modifications", "Motivation and assumptions ", "Assumptions", "Algorithm description"  },
		        /*18. Результаты*/
		        new string[] { "Results", "Research Results", "Practical results", "Case studies", "Applications", "Scale up and design" },
		        /*19. Рисунки*/
		        new string[] { "Fig", "Figure", "Illustration","Image", "Plot", "Distribution","Schematic", "Curve", "Contour","Surface" },
		        /*20. Таблицы*/
		        new string[] { "Table", "Selected data sets", "Overview", "Comparison" },
		        /*21. Аналитические теоретические результаты*/
		        new string[] { "Analytical", "Mathematical modeling", "Theoretical", "Mathematical framework" },
		        /*22. Экспериментальные результаты*/
		        new string[] { "Experimental research", "Design of experiment", "Physical modelling ", "Processing", "Performance", "Mechanical properties" },
		        /*23. Сопоставление*/
		        new string[] { "Comparison", "Evaluation" },
		        /*24. Обсуждение*/
		        new string[] { "Discussion", "Result and discussion", "Extensions and applications", "Applications and commercialization" },
		        /*25. Выводы*/
		        new string[] { "Conclusion", "Summary", "Выводы" },
		        /*26. Список литературы*/
		        new string[] { "References", "Literature", "Список литературы", "Литература" },
		        /*27. Благодарности*/
		        new string[] { "Acknowledgements", "Acknowledgments", "Grateful to", "Provided by", "Support", "Funding", "Grant", "Help", "Благодарности" },
		        /*28. Авторский вклад в работу*/
		        new string[] { "Author Contributions" },
		        /*29. Цитирование работы*/
		        new string[] { "Cite this article" },
		        /*30. Приложения к работе*/
		        new string[] { "Appendix", "Appendices", "Additional" },
		        /*31. Примечания*/
		        new string[] { "Footnotes" },
		        /*32. Дополнительные сведения об авторах*/
		        new string[] { "Authors information" } };

        public MainWindow()
        {
            typesCheck = new _TypesCheck();
            delimeters = new Char[] { '.', '!', '?', ';', ',', ':', ' ', '\t', '\r', '\n', '(', ')', '"' };

            InitializeComponent();

            var words = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\keywords.ru-en.txt").Split(new char[] { ',' });

            foreach (var word in words)
            {
                lbKeys.Items.Add(word);
            }

            var links = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\links.txt").Split(new char[] { ',' });

            foreach (var link in links)
            {
                lbKeys_Link.Items.Add(link);
            }

            //getStats();

        }

        private void AddFile_Click(string filter, RichTextBox rtb)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Открыть файл";
            ofd.Filter = filter;

            if (ofd.ShowDialog() == true)
            {
                string extension = System.IO.Path.GetExtension(ofd.FileName);

                try
                {
                    if (typesCheck.IsWord(extension))
                        _Reader.ReadDoc(ofd.FileName, rtb, sent);

                    else if (typesCheck.IsPDF(extension))
                        _Reader.ReadPDF(ofd.FileName, rtb);

                    else
                        _Reader.ReadTxt(ofd.FileName, rtb);

                }
                catch
                {
                    MessageBox.Show("Некоректный формат файла");
                }
            }

        }

        bool isRussian(string text)
        {
            int engCount = 0, rusCount = 0;
            foreach (char c in text)
            {
                if ((c > 'а' && c < 'я') || (c > 'А' && c < 'Я'))
                    rusCount++;
                else if ((c > 'a' && c < 'z') || (c > 'A' && c < 'Z'))
                    engCount++;
            }

            return rusCount > engCount;
        }

        private void mnAddFile_Click(object sender, RoutedEventArgs e)
        {
            //типы файлов для открытия
            AddFile_Click("pdf files(*.pdf)|*.pdf|doc files(*.doc)|*.doc|All files (*.*)|*.*", rtbInput);
            //строка исходного текста

            string rtbText = new TextRange(rtbInput.Document.ContentStart, rtbInput.Document.ContentEnd).Text.Replace(Environment.NewLine, String.Empty);

            //функция определения языка
            bool isRus = isRussian(rtbText);
            //открытие файла Excel в зависимости от языка
            string fName = isRus ? "rus.xlsx" : "en.xlsx";
            //ключевые слова для предметной области
            //  string[] rusWords = { "деформ", "пластич", "материал", "структур", "ипд", "давлен", "сдвиг", "металл", "процес", "метод" };
            //  string[] enWords = { "deform", "plastic", "strain", "stress", "crack", "ecap", "fractur", "mater", "plane", "structur" };

            //удаление стоп-слов из документа
            StreamReader reader = new StreamReader(System.IO.Path.Combine(Environment.CurrentDirectory, "stopwords.ru-en.txt"),
                System.Text.Encoding.Default);
            string line, rtbStopWText = "";
            while ((line = reader.ReadLine()) != null)
                rtbStopWText += line + " ";
            reader.Close();

            //частотный анализ документа
            int totalCount = wordCount(rtbText, rtbStopWText, out Characteristics.WC);
            Characteristics.WordCount = totalCount;
            ////открытие обьекта Excel 
            //Microsoft.Office.Interop.Excel.Application exApp = new Microsoft.Office.Interop.Excel.Application();
            //Microsoft.Office.Interop.Excel.Workbook wb = exApp.Workbooks.Open(
            //    System.IO.Path.Combine(Environment.CurrentDirectory, fName));
            //Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Sheets[1];

            //запись рассчитанной значимости ключевых слов в Excel

            //            for (int i = 0; i < 10; i++)
            //            {
            //                int tmp = 0;

            //                foreach (string w in wc.Keys)
            //                    if (lbKeys.Items.Count > i)
            //                        if (w.StartsWith(lbKeys.Items[i].ToString())){
            //                            tmp += wc[w];
            //                        }
            ////                ws.Cells[14 + i, 7].Value = (double)tmp / totalCount * 100;
            //            }

            //for (int i = 0; i < 10; i++)
            //{
            //    int tmp = 0;
            //    foreach (string w in wc.Keys)
            //        if (w.StartsWith(isRus ? rusWords[i] : enWords[i]))
            //            tmp += wc[w];
            //    ws.Cells[14 + i, 7].Value = (double)tmp / totalCount * 100;
            //}
            //wb.Save();
            ////вывод рассчитанного коэффициента схожести из Excel
            //lbKoef.Content = "Коэффициент схожести проанализированной статьи с группой заданных (%) - "
            //    + Math.Round(ws.Cells[45, 13].Value, 2);
            ////закрытие обьекта Excel
            //wb.Close();
            //exApp.Quit();

            get8Parts(rtbText);

            //getParts(rtbText);

        }

        void getParts(string text)
        {
            RichTextBox[] rtbs = { rtbP1, rtbP2, rtbP3, rtbP4, rtbP5, rtbP6, rtbP7, rtbP8, rtbP9, rtbP10, rtbP11, rtbP12, rtbP13, rtbP14, rtbP15, rtbP16, rtbP17, rtbP18, rtbP19, rtbP20, rtbP21, rtbP22, rtbP23, rtbP24, rtbP25, rtbP26, rtbP27, rtbP28, rtbP29, rtbP30, rtbP31, rtbP32 };

            int partitionsAmount = rtbs.Length;

            List<Partition> partitions = new List<Partition>(){
                new DOI(),
                new Classifiers(),
                new Title(),
                new Journal(),
                new PublicationPriority(),
                new Copyrights(),
                new Publisher(),
                new AcademicEditor(),
                new Annotation(),
                new Keywords(),
                new Conventions(),
                new Introduction(),
                new LiteratureReview(),
                new AimsObjectives(),
                new ResearchMethods(),
                new ProblemStatement(),
                new Results(),
                new Images(),
                new Tables(),
                new AnalyticalResult(),
                new ExperimentalResult(),
                new Matching(),
                new Discussion(),
                new Summary(),
                new References(),
                new Gratitudes(),
                new AuthorContributions(),
                new WorkCitations(),
                new WorkAttachments(),
                new Footnotes(),
                new AuthorsInformation()
            };

            for (int i = 0; i < partitionsAmount; i++)
            {
                rtbs[i].Document.Blocks.Clear();
                rtbs[i].Document.Blocks.Add(new Paragraph(new Run(partitions[i].find(text))));
            }

        }



        void fillTop(Label lab, ProgressBar pb, string labValue, int pbValue, string partitionsCount)
        {
            lab.Content = labValue;
            pb.Value = (int)((float)pbValue / Int32.Parse(partitionsCount) * 100);
        }

        void get8Parts(string text)
        {
            //переменные всех разделов
            RichTextBox[] rtbs = { rtbP1, rtbP2, rtbP3, rtbP4, rtbP5, rtbP6, rtbP7, rtbP8, rtbP9, rtbP10, rtbP11, rtbP12, rtbP13, rtbP14, rtbP15, rtbP16, rtbP17, rtbP18, rtbP19, rtbP20, rtbP21, rtbP22, rtbP23, rtbP24, rtbP25, rtbP26, rtbP27, rtbP28, rtbP29, rtbP30, rtbP31, rtbP32 };
            //номера разделов конец которых по переносу строки
            int[] spNums = { 0, 1, 3, 5, 6, 7, 18, 19 }; //10, 
            int[] dotKeyWords = { 10 };
            int ind1 = -1, ind2 = -1, tmpind;
            //очистить поля всех разделов
            for (int i = 0; i < Keys.Length; i++)
            {
                rtbs[i].Document.Blocks.Clear();
                //вызов функции поиска ключевых слов разделов
                ind1 = foundInd(Keys[i], text, 0);
                //
                while (ind1 > 0)
                {
                    ind2 = -1;
                    for (int j = 0; j < Keys.Length; j++)
                    {
                        if (j == 18) continue;
                        tmpind = foundInd(Keys[j], text, ind1 + 1);
                        if (ind2 < 0 || tmpind > 0 && tmpind < ind2)
                            ind2 = tmpind;
                    }
                    if (ind2 < 0)
                        ind2 = text.Length;

                    if (spNums.Contains(i))
                    {
                        var match = Regex.Match(text.Substring(ind1), @"[(\n|\r|\r\n)]");
                        if (match.Success && match.Index + ind1 < ind2)
                            ind2 = match.Index + ind1;
                    }

                    if (dotKeyWords.Contains(i))
                    {
                        var match = Regex.Match(text.Substring(ind1), @"[(.)]");
                        if (match.Success && match.Index + ind1 < ind2)
                            ind2 = match.Index + ind1;
                    }

                    if (ind1 > 0 && ind2 > ind1)
                    {
                        rtbs[i].AppendText(text.Substring(ind1, ind2 - ind1).Trim());
                        rtbs[i].AppendText("\r\n");
                    }

                    ind1 = foundInd(Keys[i], text, ind1 + 1);
                }

            }

        }
        //функция поиска ключевых слов разделов
        int foundInd(string[] keys, string text, int si)
        {
            int ind = -1;

            foreach (string kw in keys)
            {
                var pattern = @"((" + kw.Replace(".", "\\.") + ")|(" + kw.ToUpper().Replace(".", "\\.") + "))|(^" + kw + ")|(^" + kw.ToUpper() + ")"; //((\n|\r|\r\n).?\s?)
                var textForSearch = text.Substring(si);
                var match = Regex.Match(textForSearch, pattern);

                if (kw == "Keywords")
                {

                }

                if (match.Success)
                {
                    ind = match.Index + match.Value.ToUpper().IndexOf(kw.ToUpper()) + si;
                    break;
                }
            }

            return ind;
        }

        private void mnAddStopWFile_Click(object sender, RoutedEventArgs e)
        {
            AddFile_Click("Text files(*.txt)|*.txt|All files (*.*)|*.*", rtbStopWInputD);
        }

        private void btClearInput_Click(object sender, RoutedEventArgs e)
        {
            rtbInput.Document.Blocks.Clear();
        }

        private void btClearStopWInputD_Click(object sender, RoutedEventArgs e)
        {
            rtbStopWInputD.Document.Blocks.Clear();
        }

        private void btAddKey_Click(object sender, RoutedEventArgs e)
        {
            AddKeyDialog akd = new AddKeyDialog(this.Left + 70, this.Top + 70);

            if (akd.ShowDialog() == true)
            {
                //string[] words = akd.tb.Text.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
                lbKeys.Items.Add(akd.tb.Text);

            }
        }

        private void btDeleteKey_Click(object sender, RoutedEventArgs e)
        {
            while (lbKeys.SelectedItems.Count > 0)
            {
                lbKeys.Items.Remove(lbKeys.SelectedItem);
            }
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btGenerate_Click(object sender, RoutedEventArgs e)
        {
            //очистка результата обработки
            rtbResult.Document.Blocks.Clear();
            //строка исходного текста
            string rtbText = new TextRange(rtbInput.Document.ContentStart, rtbInput.Document.ContentEnd).Text;
            //создание таблицы слов частотного анализа
            Dictionary<string, double> wordDensity = new Dictionary<string, double>();
            //если указан расчёт значимости предложений
            if (cbDensity.IsChecked == true)
            {	//строка слов
                List<string> allwords = new List<string>(rtbText.Split(delimeters, StringSplitOptions.RemoveEmptyEntries));
                //перевод в нижний регистр слов
                for (int i = 0; i < allwords.Count; i++)
                {
                    allwords[i] = allwords[i].ToLower();
                }
                //строка слов без повторов
                List<string> uWords = new List<string>(allwords.Distinct());
                //расчёт значимости и количества слов
                foreach (string word in uWords)
                {
                    int c = allwords.FindAll(w => w.Equals(word)).Count;
                    wordDensity.Add(word, Math.Round((double)c / allwords.Count * 100, 2));
                }
                //если активен ввод количества предложений в %
                if (cbPercent.IsChecked.Value)
                {
                    double per;
                    if (Double.TryParse(tbPercent.Text, out per))
                    { //
                        wordDensity = wordDensity.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
                        for (int i = 0; i < wordDensity.Count - per; i++)
                            wordDensity[wordDensity.ElementAt(i).Key] = 0;
                    }
                }
            }


            string[] sentences = Regex.Split(rtbText, @"(?<=[.!?;])");
            string[] words;

            dt = new _DataTable();
            DataRow dr;
            //переменные для расчёта веса предложений
            int size = 0;
            double sentDdensity = 0;
            double num = 0;
            bool isCheck = getSenNum(ref num);
            //если введено количество предложений в %
            if (cbSenNumP.IsChecked.Value)
                num = Math.Round(num / 100 * sentences.Length);

            foreach (string sentence in sentences)
            {
                size = 0; sentDdensity = 0; int minW; int maxW;
                words = sentence.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
                //если количество слов в предложении больше указанного
                if (
                    (cbMinWords.IsChecked.Value && Int32.TryParse(tbMinWords.Text, out minW) && words.Length < minW)
                     ||
                    (cbMaxWords.IsChecked.Value && Int32.TryParse(tbMaxWords.Text, out maxW) && words.Length > maxW)
                    )
                    continue;
                //введенные ключевые слова
                foreach (var item in lbKeys.Items)
                {
                    string kw = item.ToString();
                    List<string> keys = new List<string>();
                    for (int i = 0; i < kw.Length; i++)
                    {
                        if (kw[i] == '\"')
                            for (int j = i + 1; j < kw.Length; j++)
                                if (kw[j] == '\"')
                                {
                                    keys.Add(kw.Substring(i + 1, j - i - 1));
                                    kw = kw.Remove(i, j - i + 1);
                                    break;
                                }
                    }
                    keys.AddRange(kw.Split(delimeters, StringSplitOptions.RemoveEmptyEntries));

                    bool isResult = true;
                    //если предложение содержит ключевые слова
                    foreach (string k in keys)
                    {
                        bool b = cbKeyReg.IsChecked == true ? sentence.Contains(k)
                            : sentence.ToLower().Contains(k.ToLower());
                        if (!b)
                            isResult = false;
                    }
                    if (isResult) size++;
                }
                //если указан расчёт значимости предложений\
                double temp;
                if (cbDensity.IsChecked == true)
                {	//для всех слов документа
                    foreach (string word in words)
                    {

                        //если слово не помечено плохим - суммирование веса слов в предложении

                        if (badWords.IndexOf(word) < 0)
                            sentDdensity += wordDensity[word.ToLower()];
                        //иначе вычитание коэффициента значимости для плохого слова
                        else

                            sentDdensity -= (wordDensity[word.ToLower()] * (Double.TryParse(tbBadWords.Text, out temp) ? temp : 2));

                    }
                    //нормализация веса предложений 
                    if (cbDensityNorm.IsChecked.Value)
                        //вес предложения разделенный на количество слов
                        sentDdensity /= words.Length;
                    //коэффициенты для стилей шрифта
                    if (sent.ContainsKey(sentence.Trim()))
                    {
                        double tmp;
                        if (sent[sentence.Trim()][0])
                            //если текст жирный
                            sentDdensity *= Double.TryParse(tbMulB.Text, out tmp) ? tmp : 1.2;
                        if (sent[sentence.Trim()][1])
                            //если текст курсивный
                            sentDdensity *= Double.TryParse(tbMulI.Text, out tmp) ? tmp : 1.1;
                    }
                }
                if (size > 0)
                {
                    dr = dt.NewRow();
                    dr["sentence"] = sentence + " "; //.Trim();
                                                     //сортировка предложений по значимости
                    dr["value"] = cbDensity.IsChecked == true ? sentDdensity : size;
                    dt.Rows.Add(dr);
                }
            }


            DataTable dt1 = dt.sortByColumn("value");
            double minval = 0;
            if (isCheck && dt1.Rows.Count > num)
                minval = (double)dt1.Rows[dt1.Rows.Count - (int)num]["value"];
            int count = dt1.Rows.Count - 1;

            if (cbDensity.IsChecked == true)
            {
                for (int i = 0, k = 0; i <= count; i++)
                {
                    if (isCheck && k >= num)
                        break;
                    if ((double)dt.Rows[i]["value"] >= minval)
                    {
                        rtbResult.AppendText(dt.Rows[i]["sentence"].ToString() + '\r');
                        k++;
                    }
                }
            }
            else
                for (int i = count; i >= 0; i--)
                {

                    if (isCheck && count - i > num - 1)
                        break;

                    dr = dt1.Rows[i];
                    rtbResult.AppendText(dr["sentence"].ToString() + '\r');
                }
        }

        private bool getSenNum(ref double num)
        {

            if (cbSenNum.IsChecked.Value)
            {
                if (Double.TryParse(tbSenNum.Text, out num))
                    return true;
            }
            else if (cbSenNumP.IsChecked.Value)
            {
                if (Double.TryParse(tbSenNumP.Text, out num))
                    return true;
            }

            return false;
        }

        private void cbSenNum_click(object sender, RoutedEventArgs e)
        {
            tbSenNum.IsEnabled = cbSenNum.IsChecked.Value;
            if (cbSenNum.IsChecked.Value)
            {
                cbSenNumP.IsChecked = false;
                tbSenNumP.IsEnabled = cbSenNumP.IsChecked.Value;
            }
        }

        private void cbSenNumP_click(object sender, RoutedEventArgs e)
        {
            tbSenNumP.IsEnabled = cbSenNumP.IsChecked.Value;
            if (cbSenNumP.IsChecked.Value)
            {
                cbSenNum.IsChecked = false;
                tbSenNum.IsEnabled = cbSenNum.IsChecked.Value;
            }
        }

        private void mnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files(*.txt)|*.txt|All(*.*)|*";


            if (sfd.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(sfd.FileName, new TextRange(
                                                rtbResult.Document.ContentStart,
                                                rtbResult.Document.ContentEnd).Text);
                }
                catch
                {
                    MessageBox.Show(String.Format("Неверный формат файла: '{0}'",
                                    sfd.FileName));
                }
            }

        }

        private void btAnalisD_Click(object sender, RoutedEventArgs e)
        {
            DataTable res = new DataTable();
            res.Columns.Add("Word");
            res.Columns.Add("Count");
            res.Columns.Add("Density");

            res.Columns.Add("+");
            res.Columns.Add("-");

            res.Columns[1].DataType = System.Type.GetType("System.Int32");
            res.Columns[3].DataType = System.Type.GetType("System.Boolean");
            res.Columns[4].DataType = System.Type.GetType("System.Boolean");

            res.Columns[0].ReadOnly = false;
            res.Columns[1].ReadOnly = false;
            res.Columns[2].ReadOnly = false;

            //строка исходного текста
            string rtbText = new TextRange(rtbInput.Document.ContentStart, rtbInput.Document.ContentEnd).Text;
            //строка стоп-слов
            string rtbStopWText = new TextRange(rtbStopWInputD.Document.ContentStart, rtbStopWInputD.Document.ContentEnd).Text;

            int c = wordCount(rtbText, rtbStopWText, out Characteristics.WC);
            //расчёт веса значимости слов
            for (int i = 0; i < Characteristics.WC.Count; i++)
            {
                res.Rows.Add(Characteristics.WC.Keys.ElementAt(i), Characteristics.WC.Values.ElementAt(i), Math.Round((double)Characteristics.WC.Values.ElementAt(i) / c * 100, 2) + "%", false, false);
            }
            //вывод частотного анализа в таблицу
            dgAnalisD.ItemsSource = res.DefaultView;
            dgAnalisD.Columns[0].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        int wordCount(string text, string stopWText, out Dictionary<string, int> res)
        {
            res = new Dictionary<string, int>();
            //удалений символов разделителей
            string[] stopWords = stopWText.Split(new string[] { "\r", "\n", " ", ",", "\t", ";" },
                StringSplitOptions.RemoveEmptyEntries);
            List<string> words = new List<string>(text.Split(delimeters, StringSplitOptions.RemoveEmptyEntries));
            //удаление всех стоп-слов в документе
            foreach (string sw in stopWords)
            {
                words.RemoveAll(w => w.ToLower().Equals(sw.ToLower()));
            }
            //переменная языка документа
            bool isRus = isRussian(text);
            IStemmer stm;
            //если русский-использовать стеммеры русского языка
            if (isRussian(text))
                stm = new RussianStemmer();
            //иначе английский
            else
                stm = new EnglishStemmer();
            //нормализация всех слов документа
            for (int i = 0; i < words.Count; i++)
            {
                words[i] = stm.Stem(words[i].ToLower());
            }
            //строка уникальных слов, без повторов
            List<string> uWords = new List<string>(words.Distinct());



            for (int i = 0; i < uWords.Count; i++)
            {
                //подсчёт количества повторений каждого слова
                res.Add(uWords[i], words.FindAll(w => w.Equals(uWords[i])).Count);
            }

            return words.Count;
        }

        private void cbPercent_Click(object sender, RoutedEventArgs e)
        {
            tbPercent.IsEnabled = cbPercent.IsChecked.Value;
        }

        private void dgAnalisD_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            object[] cells = ((DataRowView)e.Row.Item).Row.ItemArray;
            string col = e.Column.Header.ToString();

            if (col == "Density")
            {
                float value = 0;
                float.TryParse((e.EditingElement as TextBox).Text.Replace("%", String.Empty), out value);
                Characteristics.WC[cells[0].ToString()] = (int)(value * Characteristics.WordCount / 100);

                ((DataRowView)e.Row.Item).Row.SetField(1, Characteristics.WC[cells[0].ToString()]);
            }
            else
            {
                if (((CheckBox)e.EditingElement).IsChecked.Value)
                {
                    if (col.Contains("+"))
                        lbKeys.Items.Add(cells[0]);
                    else if (col.Contains("-"))
                        badWords.Add(cells[0].ToString());
                }
                else
                {
                    if (col.Contains("+"))
                        lbKeys.Items.Remove(cells[0]);
                    else if (col.Contains("-"))
                        badWords.Remove(cells[0].ToString());
                }
            }
        }

        private void btAbr_Click(object sender, RoutedEventArgs e)
        {
            string rtbText = new TextRange(rtbInput.Document.ContentStart, rtbInput.Document.ContentEnd).Text;
            string[] words = rtbText.Split(delimeters, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
            rtbAbr.Document.Blocks.Clear();
            string res = "";

            foreach (string w in words)
                if (w.Equals(w.ToUpper()) && Regex.IsMatch(w, @"[A-Z]{2,}|[А-Я]{2,}"))
                    res += w + ", ";

            if (res.Length > 2)
                rtbAbr.AppendText(res.Remove(res.Length - 2));

        }

        private void cbDensity_Click(object sender, RoutedEventArgs e)
        {
            cbDensityNorm.IsEnabled = cbDensity.IsChecked.Value;
        }

        private void cbMinWords_click(object sender, RoutedEventArgs e)
        {
            tbMinWords.IsEnabled = cbMinWords.IsChecked.Value;
        }

        private void cbMaxWords_click(object sender, RoutedEventArgs e)
        {
            tbMaxWords.IsEnabled = cbMaxWords.IsChecked.Value;
        }

        private void cbBadWords_click(object sender, RoutedEventArgs e)
        {
            tbBadWords.IsEnabled = cbBadWords.IsChecked.Value;
        }

        private void cbMulI_click(object sender, RoutedEventArgs e)
        {
            tbMulI.IsEnabled = cbMulI.IsChecked.Value;
            tbMulI.Text = "1.1";
        }

        private void cbMulB_click(object sender, RoutedEventArgs e)
        {
            tbMulB.IsEnabled = cbMulB.IsChecked.Value;
            tbMulB.Text = "1.2";
        }

        private void cbDensityNorm_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void SaveKeyWordsBtn_Click(object sender, RoutedEventArgs e)
        {
            var newText = new StringBuilder();
            foreach (var value in lbKeys.Items)
            {
                newText.Append(value);
                if (lbKeys.Items.IndexOf(value) != lbKeys.Items.Count - 1)
                {
                    newText.Append(',');
                }
            }

            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\keywords.ru-en.txt", newText.ToString(), Encoding.UTF8);
        }

        private void SaveStopWordsBtn_Click(object sender, RoutedEventArgs e)
        {
            var text = new TextRange(rtbStopWInputD.Document.ContentStart, rtbStopWInputD.Document.ContentEnd).Text;
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\stopwords.ru-en.txt", text, Encoding.UTF8);
        }

        private void SaveAricleKeyWords_Click(object sender, RoutedEventArgs e)
        {
            var articleKeyWords = new TextRange(rtbP11.Document.ContentStart, rtbP11.Document.ContentEnd).Text;

            var fileKeyWords = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\keywords.ru-en.txt");

            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\keywords.ru-en.txt", fileKeyWords.Replace("\r\n", String.Empty) + ',' + articleKeyWords, Encoding.UTF8);

            //Обновляем ListBox на главной странице, если добавляем ключевые слова из раздела в файл
            var wordsString = (fileKeyWords + ',' + articleKeyWords);
            var words = wordsString.Replace('\r', ',').Replace(".", "").Replace(", ", ",").Replace(" ", ",").Split(new char[] { ',' });
            foreach (var word in words)
            {
                if (!((word == "Keywords") || (word == "KEYWORDS") || (word == "Keywords:")) && (word != "\n") && (word.Length > 0))
                    lbKeys.Items.Add(word);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            lbKeys.Items.Clear();
        }

        private void SaveTextBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document document = winword.Documents.Add();

            var text = new TextRange(rtbInput.Document.ContentStart, rtbInput.Document.ContentEnd).Text;
            document.Content.SetRange(0, 0);
            document.Content.Text = text + Environment.NewLine;

            object filename = AppDomain.CurrentDomain.BaseDirectory + "\\test_doc.doc";
            document.SaveAs2(ref filename);
            document.Close();
            document = null;
            winword.Quit();
            winword = null;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document document = winword.Documents.Add();

            var text = new TextRange(rtbResult.Document.ContentStart, rtbResult.Document.ContentEnd).Text;
            var formatedText = Regex.Replace(text.ToString(), @"[.]\s*(\n|\r|\r\n)*", ". \n", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            document.Content.SetRange(0, 0);
            document.Content.Text = formatedText + Environment.NewLine;

            object filename = AppDomain.CurrentDomain.BaseDirectory + "\\test_abstract.doc";
            document.SaveAs2(ref filename);
            document.Close();
            document = null;
            winword.Quit();
            winword = null;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void documentsAmountTbx_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        ParserWorker parser;

        internal HindawiSettings HindawiSettings { get; private set; }
        List<Documet> listParserDocument = new List<Documet>();

        Dictionary<string, double> IDForWordsInTexts = new Dictionary<string, double>();
        Dictionary<string, double> documents = new Dictionary<string, double>();

        public void ParserHindawi(object sender, RoutedEventArgs e)
        {
            parser = new ParserWorker(
                new HindawiParser()
            );

            parser.OnCompletedParsed += Parser_OnCompletedParsed;
            parser.OnNewData += Parser_OnNewData;

            HindawiSettings = new HindawiSettings();

            string[] clist = lbKeys_Link.Items.OfType<string>().ToArray();

            HindawiSettings.BaseUrls = clist;
            parser.Settings = HindawiSettings;
            parser.Start();

        }

        private void Parser_OnNewData(object arg1, Documet arg2)
        {
            listParserDocument.Add(arg2);
        }

        private void Parser_OnCompletedParsed(object obj)
        {
            var list = listParserDocument;

            List<string> listKeyWords = new List<string>();

            listKeyWords.Add("models");
            listKeyWords.Add("Brazil");
            listKeyWords.Add("Immunology");

            checkIDForWords(list, listKeyWords.ToArray());
            CheckImportanceForDocuments(listParserDocument);

           var test = documents;
        }

        private void btAddLink_Click(object sender, RoutedEventArgs e)
        {
            AddKeyDialog akd = new AddKeyDialog(this.Left + 70, this.Top + 70);

            if (akd.ShowDialog() == true)
            {
                //string[] words = akd.tb.Text.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
                lbKeys_Link.Items.Add(akd.tb.Text);

            }
        }

        private void btDeleteLink_Click(object sender, RoutedEventArgs e)
        {
            while (lbKeys_Link.SelectedItems.Count > 0)
            {
                lbKeys_Link.Items.Remove(lbKeys_Link.SelectedItem);
            }
        }

        private void button_Click_Link(object sender, RoutedEventArgs e)
        {
            lbKeys_Link.Items.Clear();
        }

        private void SaveKeyLinksBtn_Click(object sender, RoutedEventArgs e)
        {
            var newText = new StringBuilder();
            foreach (var value in lbKeys_Link.Items)
            {
                newText.Append(value);
                if (lbKeys_Link.Items.IndexOf(value) != lbKeys_Link.Items.Count - 1)
                {
                    newText.Append(',');
                }
            }

            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\links.txt", newText.ToString(), Encoding.UTF8);
        }

        private int CountWords(string s, string s0)
        {
            int count = (s.Length - s.Replace(s0, "").Length) / s0.Length;
            return count;
        }

        private void relevanceAnalysis(string ParesedText)
        {
            //MessageBox.Show(CountWords(ParesedText, "Fortaleza").ToString());        
        }

        private void checkIDForWords(List<Documet> listParserData, Array words)
        {
            int listParserDataCount = listParserData.Count;           

            foreach (var word in words)
            {
                double IDF = 0;
                var wordString = word.ToString();
                var numberOfOccurrences = 0;

                IDForWordsInTexts.Add(wordString, 0);

                foreach (var text in listParserData)
                {
                    if (CountWords(text.parsed, wordString) > 0)
                    {
                        numberOfOccurrences += 1;
                    }

                }

                if (numberOfOccurrences != 0)
                {
                    IDF = Math.Log10(listParserDataCount / numberOfOccurrences);
                }

                IDForWordsInTexts[word.ToString()] = IDF;
            }

            //MessageBox.Show(IDForWordsInTexts["models"].ToString());
            //MessageBox.Show(IDForWordsInTexts["Brazil"].ToString());
            //MessageBox.Show(IDForWordsInTexts["11111111"].ToString());            
        }

        private void CheckImportanceForDocuments(List<Documet> Documents)
        {
            foreach (var doc in Documents)
            {                              
                double DocumentImportance = 0;
                double TFIDF = 0;

                foreach (var ifd in IDForWordsInTexts)
                {
                    double TFValue;
                    double count = CountWords(doc.Parsed, ifd.Key);
                    double DocmentLength = doc.Parsed.Length;

                    TFValue = (count / DocmentLength);
                    TFIDF += ifd.Value * TFValue;
                }

                documents.Add(doc.Url, TFIDF);
            }
        }
        
    }
}
