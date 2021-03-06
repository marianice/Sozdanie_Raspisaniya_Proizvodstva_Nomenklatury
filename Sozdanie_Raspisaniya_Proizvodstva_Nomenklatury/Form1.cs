using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace Sozdanie_Raspisaniya_Proizvodstva_Nomenklatury
{
    public partial class Form1 : Form
    {

        const int FilesCount = 4;
        String[] arrFilenames = {
                                    "Слесари.xlsx",
                                    "Номенклатура.xlsx",
                                    "Задачи.xlsx",
                                    "Время_выполнения.xlsx"
                                };        //имена файлов со входными данными

        //порядок обработки файлов
        enum eFiles
        {
            FL_TOOLS,
            FL_NOMENCLATURES,
            FL_TASKS,
            FL_TIMES
        };

        //выходной файл  
        String strOutFilename = "Расписание для слесарей" +".xlsx";

        //элемент для вывода журнала чтения файлов
        ListBox lbReadLog;

        //массивы данных
        ArrayList arrNomenclatures; //номенклатуры. Тип tNomenclature
        ArrayList arrTools;         //слесарь. Тип tMachineTool
        ArrayList arrSchedule;      //расписание. Тип tWorkSchedule
        Queue<tTasks> qTasks;     //очередь задач. Тип tTasks

        //высота формы с отчетом
        const int MaxHeight = 560;
        //высота формы по умолчанию
        const int MinHeight = 317;

        public Form1()
        {
            InitializeComponent();
        }

        //поиск слесарей по ID
        private int FindToolByID(int _ID)
        {
            for (int i = 0; i < arrTools.Count; ++i)
            {
                if (((tMachineTool)arrTools[i]).GetID() == _ID) return (i);
            }
            return (-1);
        }

        //поиск номенклатуры по ID
        private int FindNomenclatureByID(int _ID)
        {
            for (int i = 0; i < arrNomenclatures.Count; ++i)
            {
                if (((tNomenclature)arrNomenclatures[i])._id == _ID) return (i);
            }
            return (-1);
        }

        //читает очередь заадач из переданного листа Excel в очередь qTasks
        private void ReadTasks(IXLWorksheet _inSheet)
        {
            qTasks = new Queue<tTasks>();
            //первая строка - легенда, данные со второй строки
            int iRow = 2;
            //читаем, пока не найдется пустая ячейка в столбце ID
            while (_inSheet.Cell(String.Format("A{0}", iRow)).Value.ToString() != "")
            {
                tTasks Tasks = new tTasks();

                Tasks._id = Int32.Parse(_inSheet.Cell(String.Format("A{0}", iRow)).Value.ToString());
                int iNomID = Int32.Parse(_inSheet.Cell(String.Format("B{0}", iRow)).Value.ToString());

                //вставим связанную номенклатуру, найдя ее по ID
                //если номенклатура не найдена, считаем данные ошибочными и пропускаем
                int iNomIndex = FindNomenclatureByID(iNomID);
                if (iNomIndex != -1)
                {
                    Tasks._tnNomenclature = ((tNomenclature)arrNomenclatures[iNomIndex]);
                    qTasks.Enqueue(Tasks);
                }
                ++iRow;
            }
        }

        //читает словарь номенклатур из переданного листа Excel
        //в массив arrNomenclatures
        private void ReadNomenclatures(IXLWorksheet _inSheet)
        {
            arrNomenclatures = new System.Collections.ArrayList();
            //первая строка - легенда, данные со второй строки
            int iRow = 2;
            //читаем, пока не найдется пустая ячейка в столбце ID
            while (_inSheet.Cell(String.Format("A{0}", iRow)).Value.ToString() != "")
            {
                tNomenclature Nomenclature = new tNomenclature();
                Nomenclature._id = Int32.Parse(_inSheet.Cell(String.Format("A{0}", iRow)).Value.ToString());
                Nomenclature._sName = _inSheet.Cell(String.Format("B{0}", iRow)).Value.ToString();
                arrNomenclatures.Add(Nomenclature);
                ++iRow;
            }
        }

        //читает параметры оборудования: связанные номенклатуры и время их обработки
        //каждая сопоставленная номенклатура будет добавлена в соответствующий элемент типа tMachineTool массива arrTools
        private void ReadTimes(IXLWorksheet _inSheet)
        {
            //первая строка - легенда, данные со второй строки
            int iRow = 2;
            //читаем, пока не найдется пустая ячейка в столбце ID
            while (_inSheet.Cell(String.Format("A{0}", iRow)).Value.ToString() != "")
            {
                int iToolID = Int32.Parse(_inSheet.Cell(String.Format("A{0}", iRow)).Value.ToString());
                int iNomID = Int32.Parse(_inSheet.Cell(String.Format("B{0}", iRow)).Value.ToString());
                int iOpTime = Int32.Parse(_inSheet.Cell(String.Format("C{0}", iRow)).Value.ToString());

                //нашли индексы оборудования и номенклатуры в массивах
                int iToolIndex = FindToolByID(iToolID);
                int iNomIndex = FindNomenclatureByID(iNomID);
                if ((iToolIndex != -1) && (iNomIndex != -1))
                {
                    //если слесарь и номенклатура известны, то разрешаем этому оборудованию работать с этой номенклатурой за время iOpTime
                    tTime Time1 = new tTime();
                    Time1._tnNomenclature = (tNomenclature)arrNomenclatures[iNomIndex];
                    Time1._iTime = iOpTime;
                    ((tMachineTool)arrTools[iToolIndex]).AddTime(Time1);
                }
               
                ++iRow;
            }
        }

        //читает слесарей из переданного листа Excel
        //в массив arrTools
        private void ReadTools(IXLWorksheet _inSheet)
        {
            arrTools = new System.Collections.ArrayList();
            //первая строка - легенда, данные со второй строки
            int iRow = 2;
            //читаем, пока не найдется пустая ячейка в столбце ID
            while (_inSheet.Cell(String.Format("A{0}", iRow)).Value.ToString() != "")
            {
                int ID = Int32.Parse(_inSheet.Cell(String.Format("A{0}", iRow)).Value.ToString());
                String MName = _inSheet.Cell(String.Format("B{0}", iRow)).Value.ToString();
                arrTools.Add(new tMachineTool(ID, MName));
                ++iRow;
            }
        }
       
        private void Form1_Load(object sender, EventArgs e)
                {
                    strOutFilename = System.IO.Directory.GetCurrentDirectory() + "\\" + strOutFilename;

                    //поля результирующей таблицы
                    dataGridView1.Columns.Add("TasksID", "Номер задачи");
                    dataGridView1.Columns.Add("Nomenclature", "Номенклатура");
                    dataGridView1.Columns.Add("Tool", "Слесарь");
                    dataGridView1.Columns.Add("Begin", "Начало этапа обработки");
                    dataGridView1.Columns.Add("End", "Конец этапа обработки");
                    dataGridView1.Visible = false;

                    //инициализация массивов
                    arrNomenclatures = new ArrayList();
                    arrTools = new ArrayList();
                    arrSchedule = new ArrayList();
                }

        private void btnReadFiles_Click(object sender, EventArgs e)
        {
            //Спросили у пользователя, где лежат входные файлы. По умолчанию это каталог с программой.
            if (folderBrowserDialog1.SelectedPath == "")
            {
                folderBrowserDialog1.SelectedPath = System.IO.Directory.GetCurrentDirectory();
            }

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                //каталог выбран
                //скрыть элементы управления и подчистить вывод
                this.Controls.Remove(lbReadLog);
                pbReport.Visible = false;
                btnCalc.Enabled = false;
                btnExcel.Enabled = false;
                this.Height = MinHeight;
                dataGridView1.Visible = false;
                dataGridView1.Rows.Clear();
            }
            else
            {
                return;
            }

            //показываем listbox с журналом загрузки файлов
            lbReadLog = new ListBox();
            lbReadLog.Location = new Point(btnReadFiles.Location.X, btnReadFiles.Location.Y + btnReadFiles.Size.Height + 12);
            lbReadLog.Size = new Size(this.ClientSize.Width - 12 * 2, this.ClientSize.Height - 12 * 3 - btnReadFiles.Size.Height);
            this.Controls.Add(lbReadLog);

            lbReadLog.Items.Add("Начато чтение входных файлов...");
            int iCounter = 0;

            //имена файлов хранятся в глобальном массиве arrFilenames, а их назначения - в enum-е eFiles
            for (int i = 0; i < FilesCount; ++i)
            {
                String strFilename = folderBrowserDialog1.SelectedPath + '\\' + arrFilenames[i].ToString();
                if (!System.IO.File.Exists(strFilename))
                {
                    lbReadLog.Items.Add("Обнаружена ошибка: Файл \"" + arrFilenames[i].ToString() + "\" не найден в выбранном расположении.");
                    btnCalc.Enabled = false;
                }
                else
                {
                    XLWorkbook xlWbk;
                    IXLWorksheet xlSheet;
                    try
                    {
                        xlWbk = new XLWorkbook(strFilename);
                        xlSheet = xlWbk.Worksheet(1);
                    }
                    catch (System.Exception ex)
                    {
                        lbReadLog.Items.Add(String.Format("Ошибка при чтении файла {0}: {1}", arrFilenames[i].ToString(), ex.Message));
                        btnCalc.Enabled = false;
                        continue;
                    }

                    ++iCounter;

                    //добрались сюда - файл в наличии и открыт для чтения. В зависимости от содержимого файла - запускаем чтение.
                    switch (i)
                    {
                        case (int)eFiles.FL_TASKS:
                            ReadTasks(xlSheet);
                            break;
                        case (int)eFiles.FL_NOMENCLATURES:
                            ReadNomenclatures(xlSheet);
                            break;
                        case (int)eFiles.FL_TIMES:
                            ReadTimes(xlSheet);
                            break;
                        case (int)eFiles.FL_TOOLS:
                            ReadTools(xlSheet);
                            break;
                    }
                }
            }

            lbReadLog.Items.Add("Чтение файлов завершено.");
            if (iCounter != FilesCount)
            {
                lbReadLog.Items.Add("Не все файлы найдены или корректно прочитаны. Для продолжения исправьте ошибки.");
                btnCalc.Enabled = false;
                btnExcel.Enabled = false;
                return;
            }
            else
            {
                lbReadLog.Items.Add("Нажмите кнопку \"Расчет\" для запуска.");
                btnCalc.Enabled = true;
                btnExcel.Enabled = false;
            }

        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            btnReadFiles.Enabled = false;
            arrSchedule = new ArrayList();

            //для каждого элемента из очереди задач:
            while (qTasks.Count > 0)
            {
                //извлекаем элемент-задачу
                tTasks Tasks = qTasks.Dequeue();

                //сортируем массив оборудования по признаку возрастания текущей занятости
                IComparer myComparer = new tMachineToolComparerLoad();
                arrTools.Sort(myComparer);

                int i;
                for (i = 0; i < arrTools.Count; ++i)
                {
                    //находим наименее занятого слесаря, подходящую для обработки номенклатуры из задач
                    if (((tMachineTool)arrTools[i]).isSuitableForNomenclature(Tasks._tnNomenclature._id))
                    {
                        tWorkSchedule wsItem;
                        wsItem._Tasks = Tasks;
                        wsItem._Tool = (tMachineTool)arrTools[i];
                        wsItem._startTime = ((tMachineTool)arrTools[i]).GetCurrentLoad();
                        //задачу назначаем на слесаря (в ее личный список дел)
                        ((tMachineTool)arrTools[i]).AssignJob(Tasks);
                        //так и в общий план работ
                        arrSchedule.Add(wsItem);
                        break;
                    }
                }
                if (i == arrTools.Count)
                {
                    //не нашлось подходящего инструмента для обработки задачи
                    MessageBox.Show(
                        String.Format("Внимание! Для задачи №{0} ({1}) не нашлось подходящего слесаря, способного её обработать.", Tasks._id, Tasks._tnNomenclature._sName),
                        "задача не обработана",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                        );
                }
            }

            //если удалось хоть одну задачу вписать в план, то стартуем вывод результатов
            if (arrSchedule.Count > 0)
            {
                //предварительно сортируем слесарей по ID, так как перепутались в ходе назначения заданий
                IComparer myComparer = new tMachineToolComparerID();
                arrTools.Sort(myComparer);

                this.Height = MaxHeight;
                dataGridView1.Visible = true;
                ShowReport();
                PrintResult();
                btnExcel.Enabled = true;
            }
        }
        
        private void btnExcel_Click(object sender, EventArgs e)
        {
            //показываем проводник с выделенным выходным файлом
            if (System.IO.File.Exists(strOutFilename))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("explorer.exe", " /select, " + strOutFilename + ""));
            }
            else
            {
                MessageBox.Show("Файл не был создан, либо уже удалён.", "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void ShowReport()
        {
            this.Controls.Remove(lbReadLog);
            pbReport.Visible = true;

            Bitmap bmp = new Bitmap(pbReport.Width, pbReport.Height, pbReport.CreateGraphics());
            Graphics graphics = Graphics.FromImage(bmp);

            int iLegendY = 25;      //высота легенды номенклатур
            int iMarginX = 0;       //отступ слева
            int iToolNamesX = 60;   //ширина легенды оборудования

            Font font = new Font("Arial", 8);
            Pen pen = new Pen(Color.Black, 1);
            Pen pen2 = new Pen(Color.Black, 2);

            //массив цветов для обозначения номенклатур
            Color[] arrClr = {
                                 Color.Gold,
                                 Color.Silver,
                                 Color.Azure,
                                 Color.Teal,
                                 Color.SpringGreen,
                                 Color.Tan
                             };

            //рисуем легенду к графику - номенклатуры
            for (int i = 0; i < arrNomenclatures.Count; ++i)
            {
                Brush brush = new SolidBrush(arrClr[i]);
                graphics.FillRectangle(brush, iToolNamesX + iMarginX + i * 60, 0, 60, iLegendY - 5);
                graphics.DrawString(((tNomenclature)arrNomenclatures[i])._sName, font, Brushes.Black, iToolNamesX + iMarginX + i * 60 + 5, 0 + 5);
            }

            int iReportZoneHeigth = pbReport.Height - iLegendY - 20;    //высота зоны всего графика загрузки слесаря
            int iToolHeight = iReportZoneHeigth / arrTools.Count;       //высота зоны одного экземпляра слесаря

            //рассчитываем множитель масштаба по оси Х
            //нашли максимальную загрузку, добавили отступ 60, собрали коэффициент как отношение ширины клиентской зоны графика к максимальной загрузке
            float fMaxLoad = 0;
            for (int i = 0; i < arrTools.Count; ++i)
            {
                if (((tMachineTool)arrTools[i]).GetCurrentLoad() > fMaxLoad) fMaxLoad = ((tMachineTool)arrTools[i]).GetCurrentLoad();
            }
            fMaxLoad += 60;
            int iReportZoneWidth = pbReport.Width - iMarginX - iToolNamesX; //ширина зоны графика загрузки оборудования
            float fScaleCoeff = iReportZoneWidth / fMaxLoad;                //множитель масштаба

            //пишем названия слесаря и рисуем горизонтальные оси
            for (int i = 0; i < arrTools.Count; ++i)
            {
                //горизонтальная ось
                graphics.DrawLine(pen, 0, iLegendY + (i + 1) * iToolHeight, pbReport.Width, iLegendY + (i + 1) * iToolHeight);
                //название слесаря
                graphics.DrawString(((tMachineTool)arrTools[i]).GetName(), new Font("Times New Roman", 10), Brushes.DarkCyan, iMarginX, iLegendY + i * iToolHeight + iToolHeight / 2 - 5);

                //рисуем шкалу
                for (int j = 1; j <= fMaxLoad / 10; ++j)
                {
                    if (j % 5 == 0)
                    {
                        //каждая пятая засечка большая и с подписью
                        graphics.DrawLine(pen2, iToolNamesX + j * 10 * fScaleCoeff, iLegendY + (i + 1) * iToolHeight - 5, iToolNamesX + j * 10 * fScaleCoeff, iLegendY + (i + 1) * iToolHeight + 5);
                        graphics.DrawString(String.Format("{0}", j * 10), new Font("Arial Narrow", 8), Brushes.Black, iToolNamesX + j * 10 * fScaleCoeff - 2, iLegendY + (i + 1) * iToolHeight + 5);
                    }
                    else
                    {
                        graphics.DrawLine(pen, iToolNamesX + j * 10 * fScaleCoeff, iLegendY + (i + 1) * iToolHeight - 3, iToolNamesX + j * 10 * fScaleCoeff, iLegendY + (i + 1) * iToolHeight + 3);
                    }
                }
            }

            //рисуем вертикальную ось (момент времени 0)
            graphics.DrawLine(pen, iToolNamesX, iLegendY, iToolNamesX, iLegendY + iReportZoneHeigth + 20);

            int iLoadHeight = iToolHeight / 3;  //высота элемента графика загрузки
            //рисуем загрузку слесаря
            for (int i = 0; i < arrTools.Count; ++i)
            {
                int iCurrLoad = 0;
                for (int j = 0; j < ((tMachineTool)arrTools[i]).arrWorks.Count; ++j)
                {
                    tMachineTool mt = (tMachineTool)arrTools[i];
                    //цвет выбираем такой же, как был в легенде номенклатур
                    Brush brush = new SolidBrush(arrClr[((tTasks)mt.arrWorks[j])._tnNomenclature._id]);
                    //прямоугольник загрузки
                    graphics.FillRectangle(brush,
                        iToolNamesX + iCurrLoad * fScaleCoeff + 1,
                        iLegendY + (i + 1) * iToolHeight - iLoadHeight - 7,
                        mt.GetTimeByNomenclatureID(((tTasks)mt.arrWorks[j])._tnNomenclature._id) * fScaleCoeff,
                        iLoadHeight
                        );
                    //разделительная полосочка
                    graphics.DrawLine(pen,
                        iToolNamesX + (iCurrLoad) * fScaleCoeff,
                        iLegendY + (i + 1) * iToolHeight - iLoadHeight - 7,
                        iToolNamesX + (iCurrLoad) * fScaleCoeff,
                        iLegendY + (i + 1) * iToolHeight - 6
                        );
                    //номер задачи
                    graphics.DrawString(String.Format("{0}",
                        ((tTasks)mt.arrWorks[j])._id),
                        font,
                        Brushes.Black,
                        iToolNamesX + iCurrLoad * fScaleCoeff + 2,
                        iLegendY + (i + 1) * iToolHeight - iLoadHeight - 7 + 2
                        );
                    //сдвинули "каретку"
                    iCurrLoad += mt.GetTimeByNomenclatureID(((tTasks)mt.arrWorks[j])._tnNomenclature._id);
                }
            }

            pbReport.Image = bmp;
        }

        private void PrintResult()
        {
            String strFilename = strOutFilename;// System.IO.Directory.GetCurrentDirectory() + "\\" + "out.xlsx";
            if (System.IO.File.Exists(strFilename))
            {
                try
                {
                    System.IO.File.Delete(strFilename);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Внимание! Не удалось создать файл для записи результатов. Возможно, файл открыт в другой программе, или недостаточно прав для записи.", "Файл не создан", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    //return;
                }
            }
            XLWorkbook xlBook = new XLWorkbook();
            var xlSheet = xlBook.Worksheets.Add("Расписание обработки задачи");

            //вывод результатов (общая таблица планирования)
            xlSheet.Cells("A1").Value = "Номер задачи";
            xlSheet.Cells("B1").Value = "Номенклатура";
            xlSheet.Cells("C1").Value = "Слесарь";
            xlSheet.Cells("D1").Value = "Начало этапа обработки";
            xlSheet.Cells("E1").Value = "Конец этапа обработки";
            for (int i = 0; i < arrSchedule.Count; ++i)
            {
                //в excel
                xlSheet.Cells(String.Format("A{0}", i + 2)).Value = ((tWorkSchedule)arrSchedule[i])._Tasks._id;
                xlSheet.Cells(String.Format("B{0}", i + 2)).Value = ((tWorkSchedule)arrSchedule[i])._Tasks._tnNomenclature._sName;
                xlSheet.Cells(String.Format("C{0}", i + 2)).Value = ((tWorkSchedule)arrSchedule[i])._Tool.GetName();
                xlSheet.Cells(String.Format("D{0}", i + 2)).Value = ((tWorkSchedule)arrSchedule[i])._startTime;
                xlSheet.Cells(String.Format("E{0}", i + 2)).Value = ((tWorkSchedule)arrSchedule[i])._startTime + ((tWorkSchedule)arrSchedule[i])._Tool.GetTimeByNomenclatureID(((tWorkSchedule)arrSchedule[i])._Tasks._tnNomenclature._id);
                //и на экран
                dataGridView1.Rows.Add(
                    ((tWorkSchedule)arrSchedule[i])._Tasks._id,
                    ((tWorkSchedule)arrSchedule[i])._Tasks._tnNomenclature._sName,
                    ((tWorkSchedule)arrSchedule[i])._Tool.GetName(),
                    ((tWorkSchedule)arrSchedule[i])._startTime,
                    ((tWorkSchedule)arrSchedule[i])._startTime + ((tWorkSchedule)arrSchedule[i])._Tool.GetTimeByNomenclatureID(((tWorkSchedule)arrSchedule[i])._Tasks._tnNomenclature._id)
                    );
            }
            xlSheet.Columns().AdjustToContents();

            //дополнительно в отдельные страницы впишем графики загрузки каждого слесаря
            for (int i = 0; i < arrTools.Count; ++i)
            {
                String strTabName = String.Format("Расписание \"{0}\"", ((tMachineTool)arrTools[i]).GetName());
                //в excel имя воркшита лимитировано по размеру в 31 символ
                //если перебрали - заменяем полное имя на счетчик цикла
                if (strTabName.Length > 31)
                {
                    strTabName = i.ToString();
                }
                var xlSheet1 = xlBook.Worksheets.Add(strTabName);
                xlSheet1.Cells("A1").Value = "Номер задачи";
                xlSheet1.Cells("B1").Value = "Номенклатура";
                xlSheet1.Cells("C1").Value = "Начало этапа обработки";
                xlSheet1.Cells("D1").Value = "Конец этапа обработки";
                int iTime = 0;
                for (int j = 0; j < ((tMachineTool)arrTools[i]).arrWorks.Count; ++j)
                {
                    tTasks Tasks = (tTasks)(((tMachineTool)arrTools[i]).arrWorks[j]);
                    xlSheet1.Cells(String.Format("A{0}", j + 2)).Value = Tasks._id;
                    xlSheet1.Cells(String.Format("B{0}", j + 2)).Value = Tasks._tnNomenclature._sName;
                    xlSheet1.Cells(String.Format("C{0}", j + 2)).Value = iTime;
                    xlSheet1.Cells(String.Format("D{0}", j + 2)).Value = (iTime += ((tMachineTool)arrTools[i]).GetTimeByNomenclatureID(Tasks._tnNomenclature._id));
                }
                xlSheet1.Columns().AdjustToContents();
            }

            try
            {
                xlBook.SaveAs(strFilename);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Внимание! Не удалось создать файл для записи результатов. Возможно, файл открыт в другой программе, или недостаточно прав для записи.", "Файл не создан", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            btnReadFiles.Enabled = true;
            btnCalc.Enabled = false;
        }


    
       
    }
}
