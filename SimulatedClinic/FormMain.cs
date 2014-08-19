using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

namespace SimulatedClinic
{
    public partial class FormMain : Form
    {
        /*      对象：字段      */

        //读写Excel文件用
        String _currentPath;        //本exe文件所在路径
        String _desktopPath;        //桌面的路径
        FileStream _fileStream;
        IWorkbook _book;
        ISheet _sheet;
        IRow _row;
        ICell _cell;

        //数据
        DoctorDepartment[] _doctorDepartment;
        DeviceDepartment[] _deviceDepartment;
        Doctor[] _doctor;
        Device[] _device;
        Patient[] _patient;
        Int32 _patientNumber;
        Int32 _patientPointer;

        //窗体对象数组
        TabPage[] _tabPageArray;
        GroupBox[] _groupBoxArray;
        Label[] _labelWorkTimeArray;
        Label[] _labelPatientNowArray;
        ListBox[] _listBoxPatientWaitArray;
        ListBox[] _listBoxAfterCheckArray;

        //其他
        FormInput _formInput;   //导入数据窗口对象
        FormOutput _formOutput; //导出数据窗口对象
        FormThanks _formThanks; //致谢窗口对象
        Boolean _firstRefresh;  //首次更新窗体标志
        Random _random;         //随机数生成器（随即生成患者的就诊或检查时间）
        Int32 _time;            //全局时间
        Boolean _allFinish;     //是否全部结束

        /*      对象：辅助方法      */

        //读入数据
        public Int32 ReadData()
        {
            FileStream finTemp;
            while (true)
            {
                try
                {
                    finTemp = new FileStream(_currentPath + @"\Data\PatientInfo.xls", FileMode.Open);
                    break;
                }
                catch (Exception)
                {
                    DialogResult result = MessageBox.Show("找不到文件“PatientInfo.xls”！\r\n请确保其位于下列目录中：\r\n" + _currentPath + "\\Data", "错误", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Cancel)
                    {
                        return -1;
                    }
                }
            }
            IWorkbook tempBook = new HSSFWorkbook(finTemp);
            Int32 temp1 = Convert.ToInt32(tempBook.GetSheet("患者").GetRow(1).GetCell(0).ToString());
            Int32 temp2 = 0;
            DoctorDepartment pointerDoc = null;
            DeviceDepartment pointerDev = null;

            //获取患者总数
            _patientNumber = temp1;

            //获取进度条最大值
            _formInput.SetProcessBarMaximum(temp1 += (15 + 1 + 45 + 3));

            //释放资源
            finTemp.Close();
            finTemp = null;
            tempBook = null;

            //工作簿：ClinicInfo.xls
            while (true)
            {
                try
                {
                    _fileStream = new FileStream(_currentPath + @"\Data\ClinicInfo.xls", FileMode.Open);
                    break;
                }
                catch (Exception)
                {
                    DialogResult result = MessageBox.Show("找不到文件“ClinicInfo.xls”！\r\n请确保其位于下列目录中：\r\n" + _currentPath + "\\Data", "错误", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Cancel)
                    {
                        return -1;
                    }
                }
            }
            _book = new HSSFWorkbook(_fileStream);

            try
            {
                //工作表：医生科室
                _doctorDepartment = new DoctorDepartment[16];
                _sheet = _book.GetSheet("医生科室");
                for (Int32 i = 1; i <= 15; i++)
                {
                    _doctorDepartment[i] = new DoctorDepartment("", "");
                    //行：i+1
                    _row = _sheet.GetRow(i);
                    //单元格：A(i+1)
                    _cell = _row.GetCell(0);
                    _doctorDepartment[i].SetId(_cell.ToString());
                    //单元格：B(i+1)
                    _cell = _row.GetCell(1);
                    _doctorDepartment[i].SetName(_cell.ToString());
                    //进度条
                    if ((temp2 += 1) <= temp1)
                    {
                        _formInput.SetProcessBarValue(temp2);
                    }
                    else
                    {
                        _formInput.SetProcessBarValue(temp1);
                    }
                }

                //工作表：设备科室
                _deviceDepartment = new DeviceDepartment[2];
                _sheet = _book.GetSheet("设备科室");
                for (Int32 i = 1; i <= 1; i++)
                {
                    _deviceDepartment[i] = new DeviceDepartment("", "");
                    //行：i+1
                    _row = _sheet.GetRow(i);
                    //单元格：A(i+1)
                    _cell = _row.GetCell(0);
                    _deviceDepartment[i].SetId(_cell.ToString());
                    //单元格：B(i+1)
                    _cell = _row.GetCell(1);
                    _deviceDepartment[i].SetName(_cell.ToString());
                    //进度条
                    if ((temp2 += 1) <= temp1)
                    {
                        _formInput.SetProcessBarValue(temp2);
                    }
                    else
                    {
                        _formInput.SetProcessBarValue(temp1);
                    }
                }

                //工作表：医生
                _doctor = new Doctor[46];
                _sheet = _book.GetSheet("医生");
                for (Int32 i = 1; i <= 45; i++)
                {
                    _doctor[i] = new Doctor("", "", null, "");
                    //行：i+1
                    _row = _sheet.GetRow(i);
                    //单元格：A(i+1)
                    _cell = _row.GetCell(0);
                    _doctor[i].SetId(_cell.ToString());
                    //单元格：B(i+1)
                    _cell = _row.GetCell(1);
                    _doctor[i].SetName(_cell.ToString());
                    //单元格：C(i+1)
                    _cell = _row.GetCell(2);
                    switch (_cell.ToString())
                    {
                        case "呼吸内科":
                            _doctor[i].SetDepartment(pointerDoc = _doctorDepartment[1]);
                            break;
                        case "消化内科":
                            _doctor[i].SetDepartment(pointerDoc = _doctorDepartment[2]);
                            break;
                        case "泌尿内科":
                            _doctor[i].SetDepartment(pointerDoc = _doctorDepartment[3]);
                            break;
                        case "心内科":
                            _doctor[i].SetDepartment(pointerDoc = _doctorDepartment[4]);
                            break;
                        case "血液科":
                            _doctor[i].SetDepartment(pointerDoc = _doctorDepartment[5]);
                            break;
                        case "内分泌科":
                            _doctor[i].SetDepartment(pointerDoc = _doctorDepartment[6]);
                            break;
                        case "神经内科":
                            _doctor[i].SetDepartment(pointerDoc = _doctorDepartment[7]);
                            break;
                        case "感染科":
                            _doctor[i].SetDepartment(pointerDoc = _doctorDepartment[8]);
                            break;
                        case "普外科":
                            _doctor[i].SetDepartment(pointerDoc = _doctorDepartment[9]);
                            break;
                        case "骨科":
                            _doctor[i].SetDepartment(pointerDoc = _doctorDepartment[10]);
                            break;
                        case "神经外科":
                            _doctor[i].SetDepartment(pointerDoc = _doctorDepartment[11]);
                            break;
                        case "肝胆外科":
                            _doctor[i].SetDepartment(pointerDoc = _doctorDepartment[12]);
                            break;
                        case "泌尿外科":
                            _doctor[i].SetDepartment(pointerDoc = _doctorDepartment[13]);
                            break;
                        case "烧伤科":
                            _doctor[i].SetDepartment(pointerDoc = _doctorDepartment[14]);
                            break;
                        case "妇产科":
                            _doctor[i].SetDepartment(pointerDoc = _doctorDepartment[15]);
                            break;
                        default:
                            _doctor[i].SetDepartment(null);
                            break;
                    }
                    if (pointerDoc != null)
                    {
                        pointerDoc.GetDoctor()[(i - 1) % 3 + 1] = _doctor[i];
                    }
                    //单元格：D(i+1)
                    _cell = _row.GetCell(3);
                    _doctor[i].SetOther(_cell.ToString());
                    //进度条
                    if ((temp2 += 1) <= temp1)
                    {
                        _formInput.SetProcessBarValue(temp2);
                    }
                    else
                    {
                        _formInput.SetProcessBarValue(temp1);
                    }
                }

                //工作表：设备
                _device = new Device[4];
                _sheet = _book.GetSheet("设备");
                for (Int32 i = 1; i <= 3; i++)
                {
                    _device[i] = new Device("", "", null, "");
                    //行：i+1
                    _row = _sheet.GetRow(i);
                    //单元格：A(i+1)
                    _cell = _row.GetCell(0);
                    _device[i].SetId(_cell.ToString());
                    //单元格：B(i+1)
                    _cell = _row.GetCell(1);
                    _device[i].SetName(_cell.ToString());
                    //单元格：C(i+1)
                    _cell = _row.GetCell(2);
                    switch (_cell.ToString())
                    {
                        case "B超检查室":
                            _device[i].SetDepartment(pointerDev = _deviceDepartment[1]);
                            break;
                        default:
                            _device[i].SetDepartment(null);
                            break;
                    }
                    if (pointerDev != null)
                    {
                        pointerDev.GetDevice()[(i - 1) % 3 + 1] = _device[i];
                    }
                    //单元格：D(i+1)
                    _cell = _row.GetCell(3);
                    _device[i].SetOther(_cell.ToString());
                    //进度条
                    if ((temp2 += 1) <= temp1)
                    {
                        _formInput.SetProcessBarValue(temp2);
                    }
                    else
                    {
                        _formInput.SetProcessBarValue(temp1);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("文件“ClinicInfo.xls”格式不正确！\r\n程序即将退出。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -2;
            }

            //关闭ClinicInfo.xls
            _fileStream.Close();

            //工作簿：PatientInfo.xls
            while (true)
            {
                try
                {
                    _fileStream = new FileStream(_currentPath + @"\Data\PatientInfo.xls", FileMode.Open);
                    break;
                }
                catch (Exception)
                {
                    DialogResult result = MessageBox.Show("找不到文件“PatientInfo.xls”！\r\n请确保其位于下列目录中：\r\n" + _currentPath + "\\Data", "错误", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Cancel)
                    {
                        return -1;
                    }
                }
            }
            _book = new HSSFWorkbook(_fileStream);

            try
            {
                //工作表：患者
                _sheet = _book.GetSheet("患者");
                _patient = new Patient[Convert.ToInt32(_sheet.GetRow(1).GetCell(0).ToString()) + 1];
                for (Int32 i = 1; i <= Convert.ToInt32(_sheet.GetRow(1).GetCell(0).ToString()); i++)
                {
                    _patient[i] = new Patient("", "", Sex.Other, 0, "", null);
                    //行：i+3
                    _row = _sheet.GetRow(i + 3);
                    //单元格：A(i+3)
                    _cell = _row.GetCell(0);
                    _patient[i].SetId(_cell.ToString());
                    //单元格：B(i+3)
                    _cell = _row.GetCell(1);
                    _patient[i].SetName(_cell.ToString());
                    //单元格：C(i+3)
                    _cell = _row.GetCell(2);
                    switch (_cell.ToString())
                    {
                        case "男":
                            _patient[i].SetSex(Sex.Male);
                            break;
                        case "女":
                            _patient[i].SetSex(Sex.Female);
                            break;
                        default:
                            _patient[i].SetSex(Sex.Other);
                            break;
                    }
                    //单元格：D(i+3)
                    _cell = _row.GetCell(3);
                    _patient[i].SetAge(Convert.ToInt32(_cell.ToString()));
                    //单元格：E(i+3)
                    _cell = _row.GetCell(4);
                    _patient[i].SetOther(_cell.ToString());
                    //单元格：F(i+3)
                    _cell = _row.GetCell(5);
                    switch (_cell.ToString())
                    {
                        case "呼吸内科":
                            _patient[i].SetDoctorDepartment(_doctorDepartment[1]);
                            break;
                        case "消化内科":
                            _patient[i].SetDoctorDepartment(_doctorDepartment[2]);
                            break;
                        case "泌尿内科":
                            _patient[i].SetDoctorDepartment(_doctorDepartment[3]);
                            break;
                        case "心内科":
                            _patient[i].SetDoctorDepartment(_doctorDepartment[4]);
                            break;
                        case "血液科":
                            _patient[i].SetDoctorDepartment(_doctorDepartment[5]);
                            break;
                        case "内分泌科":
                            _patient[i].SetDoctorDepartment(_doctorDepartment[6]);
                            break;
                        case "神经内科":
                            _patient[i].SetDoctorDepartment(_doctorDepartment[7]);
                            break;
                        case "感染科":
                            _patient[i].SetDoctorDepartment(_doctorDepartment[8]);
                            break;
                        case "普外科":
                            _patient[i].SetDoctorDepartment(_doctorDepartment[9]);
                            break;
                        case "骨科":
                            _patient[i].SetDoctorDepartment(_doctorDepartment[10]);
                            break;
                        case "神经外科":
                            _patient[i].SetDoctorDepartment(_doctorDepartment[11]);
                            break;
                        case "肝胆外科":
                            _patient[i].SetDoctorDepartment(_doctorDepartment[12]);
                            break;
                        case "泌尿外科":
                            _patient[i].SetDoctorDepartment(_doctorDepartment[13]);
                            break;
                        case "烧伤科":
                            _patient[i].SetDoctorDepartment(_doctorDepartment[14]);
                            break;
                        case "妇产科":
                            _patient[i].SetDoctorDepartment(_doctorDepartment[15]);
                            break;
                        default:
                            _patient[i].SetDoctorDepartment(null);
                            break;
                    }
                    //进度条
                    if ((temp2 += 1) <= temp1)
                    {
                        _formInput.SetProcessBarValue(temp2);
                    }
                    else
                    {
                        _formInput.SetProcessBarValue(temp1);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("文件“PatientInfo.xls”格式不正确！\r\n程序即将退出。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -2;
            }

            //关闭PatientInfo.xls
            _fileStream.Close();

            //释放资源
            _fileStream = null;
            _book = null;
            _sheet = null;
            _row = null;
            _cell = null;

            return 1;
        }

        //写出数据
        public Int32 WriteData()
        {
            Int32 temp1 = _patientNumber + 45;
            Int32 temp2 = 0;
            Int32 temp3 = 0;
            Int32[] insectRow = new Int32[46];
            IFont fontDoc;
            IFont font;
            ICellStyle styleDoc;
            ICellStyle style;

            //获取进度条最大值
            _formOutput.SetProcessBarMaximum(temp1);

            //创建DoctorTreat.xls
            while (true)
            {
                try
                {
                    _fileStream = new FileStream(_desktopPath + @"\DoctorTreat.xls", FileMode.Create);
                    break;
                }
                catch (Exception)
                {
                    DialogResult result = MessageBox.Show("文件“DoctorTreat.xls”正在被使用，无法修改！\r\n请关闭相关的应用程序。", "错误", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Cancel)
                    {
                        return -1;
                    }
                }
            }
            _book = new HSSFWorkbook();

            //设定单元格字体
            fontDoc = _book.CreateFont();
            fontDoc.FontName = "宋体";
            fontDoc.FontHeightInPoints = 12;
            fontDoc.Boldweight = short.MaxValue;
            font = _book.CreateFont();
            font.FontName = "宋体";
            font.FontHeightInPoints = 12;

            //设置单元格样式
            styleDoc = _book.CreateCellStyle();
            styleDoc.SetFont(fontDoc);
            styleDoc.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            styleDoc.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            style = _book.CreateCellStyle();
            style.SetFont(font);
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            //输出医生信息和患者信息表头
            for (Int32 i = 1; i <= 45; i++)
            {
                //新建工作簿
                _sheet = _book.CreateSheet("DOC" + String.Format("{0:000}", i));

                //设定列宽
                _sheet.SetColumnWidth(0, 20 * 256);     //列：A
                _sheet.SetColumnWidth(1, 30 * 256);     //列：B
                _sheet.SetColumnWidth(2, 15 * 256);     //列：C
                _sheet.SetColumnWidth(3, 15 * 256);     //列：D
                _sheet.SetColumnWidth(4, 15 * 256);     //列：E
                _sheet.SetColumnWidth(5, 15 * 256);     //列：F
                _sheet.SetColumnWidth(6, 15 * 256);     //列：G
                _sheet.SetColumnWidth(7, 12 * 256);     //列：H
                _sheet.SetColumnWidth(8, 15 * 256);     //列：I
                _sheet.SetColumnWidth(9, 30 * 256);     //列：J
                _sheet.SetColumnWidth(10, 12 * 256);    //列：K

                //行：1
                _row = _sheet.CreateRow(0);
                //单元格：A1
                _cell = _row.CreateCell(0);
                _cell.SetCellValue("医生编号");
                _cell.CellStyle = styleDoc;
                //单元格：B1
                _cell = _row.CreateCell(1);
                _cell.SetCellValue(_doctor[i].GetId());
                _cell.CellStyle = styleDoc;
                //行：2
                _row = _sheet.CreateRow(1);
                //单元格：A2
                _cell = _row.CreateCell(0);
                _cell.SetCellValue("医生姓名");
                _cell.CellStyle = styleDoc;
                //单元格：B2
                _cell = _row.CreateCell(1);
                _cell.SetCellValue(_doctor[i].GetName());
                _cell.CellStyle = styleDoc;
                //行：3
                _row = _sheet.CreateRow(2);
                //单元格：A3
                _cell = _row.CreateCell(0);
                _cell.SetCellValue("医生所属科室");
                _cell.CellStyle = styleDoc;
                //单元格：B3
                _cell = _row.CreateCell(1);
                _cell.SetCellValue(_doctor[i].GetDepartment().GetName());
                _cell.CellStyle = styleDoc;
                //行：4
                _row = _sheet.CreateRow(3);
                //单元格：A4
                _cell = _row.CreateCell(0);
                _cell.SetCellValue("医生的其他信息");
                _cell.CellStyle = styleDoc;
                //单元格：B4
                _cell = _row.CreateCell(1);
                _cell.SetCellValue(_doctor[i].GetOther());
                _cell.CellStyle = styleDoc;
                //行：5
                _row = _sheet.CreateRow(4);
                //单元格：A5
                _cell = _row.CreateCell(0);
                _cell.SetCellValue("医生的工作时间");
                _cell.CellStyle = styleDoc;
                //单元格：B5
                _cell = _row.CreateCell(1);
                _cell.SetCellValue(_doctor[i].GetWorkTime());
                _cell.CellStyle = styleDoc;
                //行：6
                _row = _sheet.CreateRow(5);
                //行：7
                _row = _sheet.CreateRow(6);
                //单元格：A7
                _cell = _row.CreateCell(0);
                _cell.SetCellValue("患者编号");
                _cell.CellStyle = style;
                //单元格：B7
                _cell = _row.CreateCell(1);
                _cell.SetCellValue("患者姓名");
                _cell.CellStyle = style;
                //单元格：C7
                _cell = _row.CreateCell(2);
                _cell.SetCellValue("患者性别");
                _cell.CellStyle = style;
                //单元格：D7
                _cell = _row.CreateCell(3);
                _cell.SetCellValue("患者年龄");
                _cell.CellStyle = style;
                //单元格：E7
                _cell = _row.CreateCell(4);
                _cell.SetCellValue("患者其他信息");
                _cell.CellStyle = style;
                //单元格：F7
                _cell = _row.CreateCell(5);
                _cell.SetCellValue("患者就诊科室");
                _cell.CellStyle = style;
                //单元格：G7
                _cell = _row.CreateCell(6);
                _cell.SetCellValue("医生");
                _cell.CellStyle = style;
                //单元格：H7
                _cell = _row.CreateCell(7);
                _cell.SetCellValue("问诊时间");
                _cell.CellStyle = style;
                //单元格：I7
                _cell = _row.CreateCell(8);
                _cell.SetCellValue("患者检测科室");
                _cell.CellStyle = style;
                //单元格：J7
                _cell = _row.CreateCell(9);
                _cell.SetCellValue("设备");
                _cell.CellStyle = style;
                //单元格：K7
                _cell = _row.CreateCell(10);
                _cell.SetCellValue("检查时间");
                _cell.CellStyle = style;

                //后续数据自行8开始插入
                insectRow[i] = 7;

                //进度条
                if ((temp2 += 1) <= temp1)
                {
                    _formOutput.SetProcessBarValue(temp2);
                }
                else
                {
                    _formOutput.SetProcessBarValue(temp1);
                }
            }

            //输出患者信息
            for (Int32 i = 1; i <= _patientNumber; i++)
            {
                if (_patient[i].GetDoctor() != null)
                {
                    //选定需要输出的Sheet
                    _sheet = _book.GetSheet(_patient[i].GetDoctor().GetId());
                    //新建行x
                    temp3 = Convert.ToInt32(_patient[i].GetDoctor().GetId().Substring(3, 3));
                    _row = _sheet.CreateRow(insectRow[temp3]);
                    insectRow[temp3] += 1;
                    //单元格：A7
                    _cell = _row.CreateCell(0);
                    if (_patient[i].GetId() != null)
                    {
                        _cell.SetCellValue(_patient[i].GetId());
                    }
                    else
                    {
                        _cell.SetCellValue("");
                    }
                    _cell.CellStyle = style;
                    //单元格：B7
                    _cell = _row.CreateCell(1);
                    if (_patient[i].GetName() != null)
                    {
                        _cell.SetCellValue(_patient[i].GetName());
                    }
                    else
                    {
                        _cell.SetCellValue("");
                    }
                    _cell.CellStyle = style;
                    //单元格：C7
                    _cell = _row.CreateCell(2);
                    if (_patient[i].GetSex() == Sex.Male)
                    {
                        _cell.SetCellValue("男");
                    }
                    else if (_patient[i].GetSex() == Sex.Female)
                    {
                        _cell.SetCellValue("女");
                    }
                    else
                    {
                        _cell.SetCellValue("其他");
                    }
                    _cell.CellStyle = style;
                    //单元格：D7
                    _cell = _row.CreateCell(3);
                    _cell.SetCellValue(_patient[i].GetAge());
                    _cell.CellStyle = style;
                    //单元格：E7
                    _cell = _row.CreateCell(4);
                    if (_patient[i].GetName() != null)
                    {
                        _cell.SetCellValue(_patient[i].GetOther());
                    }
                    else
                    {
                        _cell.SetCellValue("");
                    }
                    _cell.CellStyle = style;
                    //单元格：F7
                    _cell = _row.CreateCell(5);
                    if (_patient[i].GetDoctorDepartment() != null)
                    {
                        if (_patient[i].GetDoctorDepartment().GetName() != null)
                        {
                            _cell.SetCellValue(_patient[i].GetDoctorDepartment().GetName());
                        }
                        else
                        {
                            _cell.SetCellValue("");
                        }
                    }
                    else
                    {
                        _cell.SetCellValue("");
                    }
                    _cell.CellStyle = style;
                    //单元格：G7
                    _cell = _row.CreateCell(6);
                    if (_patient[i].GetDoctor() != null)
                    {
                        if (_patient[i].GetDoctor().GetName() != null)
                        {
                            _cell.SetCellValue(_patient[i].GetDoctor().GetName());
                        }
                        else
                        {
                            _cell.SetCellValue("");
                        }
                    }
                    else
                    {
                        _cell.SetCellValue("");
                    }
                    _cell.CellStyle = style;
                    //单元格：H7
                    _cell = _row.CreateCell(7);
                    if (_patient[i].GetDoctorTime() != 0)
                    {
                        _cell.SetCellValue(_patient[i].GetDoctorTime());
                    }
                    else
                    {
                        _cell.SetCellValue("");
                    }
                    _cell.CellStyle = style;
                    //单元格：I7
                    _cell = _row.CreateCell(8);
                    if (_patient[i].GetDeviceDepartment() != null)
                    {
                        if (_patient[i].GetDeviceDepartment().GetName() != null)
                        {
                            _cell.SetCellValue(_patient[i].GetDeviceDepartment().GetName());
                        }
                        else
                        {
                            _cell.SetCellValue("");
                        }
                    }
                    else
                    {
                        _cell.SetCellValue("");
                    }
                    _cell.CellStyle = style;
                    //单元格：J7
                    _cell = _row.CreateCell(9);
                    if (_patient[i].GetDevice() != null)
                    {
                        if (_patient[i].GetDevice().GetName() != null)
                        {
                            _cell.SetCellValue(_patient[i].GetDevice().GetName());
                        }
                        else
                        {
                            _cell.SetCellValue("");
                        }
                    }
                    else
                    {
                        _cell.SetCellValue("");
                    }
                    _cell.CellStyle = style;
                    //单元格：K7
                    _cell = _row.CreateCell(10);
                    if (_patient[i].GetDeviceTime() != 0)
                    {
                        _cell.SetCellValue(_patient[i].GetDeviceTime());
                    }
                    else
                    {
                        _cell.SetCellValue("");
                    }
                    _cell.CellStyle = style;
                    //进度条
                    if ((temp2 += 1) <= temp1)
                    {
                        _formOutput.SetProcessBarValue(temp2);
                    }
                    else
                    {
                        _formOutput.SetProcessBarValue(temp1);
                    }
                }
            }

            //保存DoctorTreat.xls
            _book.Write(_fileStream);

            //关闭DoctorTreat.xls
            _fileStream.Close();

            //释放资源
            _fileStream = null;
            _book = null;
            _sheet = null;
            _row = null;
            _cell = null;

            return 1;
        }

        //设定“导入数据”按钮的“Enabled”属性
        public void SetButtonInput(Boolean enabled)
        {
            buttonInput.Enabled = enabled;
        }

        //设定“导出数据”按钮的“Exabled”属性
        public void SetButtonOutput(Boolean enabled)
        {
            buttonOutput.Enabled = enabled;
        }

        //将所有控件的“Exabled”属性设为“false”
        public void DisenableAll(Boolean quit)
        {
            buttonInput.Enabled = false;
            buttonOutput.Enabled = false;
            buttonStart.Enabled = false;
            buttonReset.Enabled = false;
            buttonOutput.Enabled = false;
            label191.Enabled = false;
            listBoxRegistration.Enabled = false;
            tabControlDepartment.Enabled = false;
            groupBoxTimeAdd.Enabled = false;
            if (quit)
            {
                buttonQuit.Enabled = false;
            }
        }

        //设定“开始运行”按钮的“Enabled”属性
        public void SetButtonStart(Boolean enabled)
        {
            buttonStart.Enabled = enabled;
        }

        //更新窗体呈现的信息
        private void refreshForm()
        {
            if (_firstRefresh)  //首次更新窗体
            {
                //初始化全局时间
                labelTIme.Text = _time.ToString();
                //初始化“排队挂号的患者序列”ListBox对象的显示内容
                for (Int32 i = 1; i <= _patientNumber; i++)
                {
                    listBoxRegistration.Items.Add(_patient[i].GetId() + " " + _patient[i].GetName());
                }
                //初始化所有TabPage对象的显示内容
                for (Int32 i = 1; i <= 15; i++)
                {
                    _tabPageArray[i].Text = _doctorDepartment[i].GetName();
                }
                _tabPageArray[16].Text = _deviceDepartment[1].GetName();
                //初始化所有GroupBox对象的显示内容
                for (Int32 i = 1; i <= 45; i++)
                {
                    _groupBoxArray[i].Text = "医生：" + _doctor[i].GetName();
                }
                for (Int32 i = 1; i <= 3; i++)
                {
                    _groupBoxArray[i + 45].Text = _device[i].GetName();
                }
                //初始化所有动态Label对象的显示内容
                for (Int32 i = 1; i <= 48; i++)
                {
                    _labelWorkTimeArray[i].Text = "0";
                    _labelPatientNowArray[i].Text = "无";
                }
                //设置首次更新窗体标志
                _firstRefresh = false;
                //显示不可见的窗体元素
                buttonReset.Visible = true;
                buttonOutput.Visible = true;
                label191.Visible = true;
                listBoxRegistration.Visible = true;
                tabControlDepartment.Visible = true;
                label192.Visible = true;
                labelTIme.Visible = true;
                groupBoxTimeAdd.Visible = true;
            }
            else  //非首次更新窗体
            {
                Patient temp = null;
                //更新全局时间
                labelTIme.Text = _time.ToString();
                //更新“排队挂号的患者序列”ListBox对象的显示内容
                listBoxRegistration.Items.Clear();
                for (Int32 i = _patientPointer + 1; i <= _patientNumber; i++)
                {
                    listBoxRegistration.Items.Add(_patient[i].GetId() + " " + _patient[i].GetName());
                }
                //更新所有“医生”GroupBox对象的显示内容
                for (Int32 i = 1; i <= 45; i++)
                {
                    //更新“工作时间”动态Label对象的显示内容
                    _labelWorkTimeArray[i].Text = _doctor[i].GetWorkTime().ToString();
                    //更新“当前患者”动态Label对象的显示内容
                    _labelPatientNowArray[i].Text = (_doctor[i].GetPatientNow() == null ? "无" : _doctor[i].GetPatientNow().GetId() + " " + _doctor[i].GetPatientNow().GetName());
                    //更新“等待就诊”动态ListBox对象的显示内容
                    _listBoxPatientWaitArray[i].Items.Clear();
                    for (Int32 j = 1; ; j++)
                    {
                        temp = _doctor[i].GetPatientWait().IndexToValue(j);
                        if (temp == null)
                        {
                            break;
                        }
                        else
                        {
                            _listBoxPatientWaitArray[i].Items.Add(temp.GetId() + " " + temp.GetName());
                        }
                    }
                    //更新“检查完毕，等待复诊”动态ListBox对象的显示内容
                    _listBoxAfterCheckArray[i].Items.Clear();
                    for (Int32 j = 1; ; j++)
                    {
                        temp = _doctor[i].GetAfterCheck().IndexToValue(j);
                        if (temp == null)
                        {
                            break;
                        }
                        else
                        {
                            _listBoxAfterCheckArray[i].Items.Add(temp.GetId() + " " + temp.GetName());
                        }
                    }
                }
                //更新所有“设备”GroupBox对象的显示内容
                for (Int32 i = 1; i <= 3; i++)
                {
                    //更新“工作时间”动态Label对象的显示内容
                    _labelWorkTimeArray[i + 45].Text = _device[i].GetWorkTime().ToString();
                    //更新“当前患者”动态Label对象的显示内容
                    _labelPatientNowArray[i + 45].Text = (_device[i].GetPatientNow() == null ? "无" : _device[i].GetPatientNow().GetId() + " " + _device[i].GetPatientNow().GetName());
                    //更新“等待就诊”动态ListBox对象的显示内容
                    _listBoxPatientWaitArray[i + 45].Items.Clear();
                    for (Int32 j = 1; ;j++)
                    {
                        temp = _device[i].GetPatientWait().IndexToValue(j);
                        if (temp == null)
                        {
                            break;
                        }
                        else
                        {
                            _listBoxPatientWaitArray[i + 45].Items.Add(temp.GetId() + " " + temp.GetName());
                        }
                    }
                }
            }
        }

        //显示患者信息
        private void displayPatientMessage(String str)
        {
            if (str == null)
            {
                return;
            }
            Int32 i;
            i = Convert.ToInt32(str.Substring(3, 3));
            MessageBox.Show(_patient[i].ReceiveInformation(), "患者信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //随机生成医生问诊的时间
        private Int32 timeDoctor()
        {
            return _random.Next(2, 10);
        }

        //随机生成患者检查的时间
        private Int32 timeDevice()
        {
            return _random.Next(4, 8);
        }

        //取消程序对键盘上一些按键的响应
        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Left:
                case Keys.Down:
                case Keys.Right:
                case Keys.Tab:
                case Keys.Control:
                case Keys.Alt:
                case Keys.Escape:
                    return true;
                default:
                    return base.ProcessDialogKey(keyData);
            }
        }

        //禁用窗体的关闭按钮
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | 0x200;
                return myCp;
            }
        }

        /*      对象：构造方法和事件      */

        public FormMain()
        {
            InitializeComponent();

            //Excel文件读写准备

            //获取当前exe文件的路径
            _currentPath = System.Windows.Forms.Application.StartupPath;
            //获取桌面的路径
            _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //初始化窗体对象数组

            _tabPageArray = new TabPage[17];
            _tabPageArray[0] = null;
            _tabPageArray[1] = tabPageDep1001;
            _tabPageArray[2] = tabPageDep1002;
            _tabPageArray[3] = tabPageDep1003;
            _tabPageArray[4] = tabPageDep1004;
            _tabPageArray[5] = tabPageDep1005;
            _tabPageArray[6] = tabPageDep1006;
            _tabPageArray[7] = tabPageDep1007;
            _tabPageArray[8] = tabPageDep1008;
            _tabPageArray[9] = tabPageDep1009;
            _tabPageArray[10] = tabPageDep1010;
            _tabPageArray[11] = tabPageDep1011;
            _tabPageArray[12] = tabPageDep1012;
            _tabPageArray[13] = tabPageDep1013;
            _tabPageArray[14] = tabPageDep1014;
            _tabPageArray[15] = tabPageDep1015;
            _tabPageArray[16] = tabPageDep2001;

            _groupBoxArray = new GroupBox[49];
            _groupBoxArray[0] = null;
            _groupBoxArray[1] = groupBoxDoc001;
            _groupBoxArray[2] = groupBoxDoc002;
            _groupBoxArray[3] = groupBoxDoc003;
            _groupBoxArray[4] = groupBoxDoc004;
            _groupBoxArray[5] = groupBoxDoc005;
            _groupBoxArray[6] = groupBoxDoc006;
            _groupBoxArray[7] = groupBoxDoc007;
            _groupBoxArray[8] = groupBoxDoc008;
            _groupBoxArray[9] = groupBoxDoc009;
            _groupBoxArray[10] = groupBoxDoc010;
            _groupBoxArray[11] = groupBoxDoc011;
            _groupBoxArray[12] = groupBoxDoc012;
            _groupBoxArray[13] = groupBoxDoc013;
            _groupBoxArray[14] = groupBoxDoc014;
            _groupBoxArray[15] = groupBoxDoc015;
            _groupBoxArray[16] = groupBoxDoc016;
            _groupBoxArray[17] = groupBoxDoc017;
            _groupBoxArray[18] = groupBoxDoc018;
            _groupBoxArray[19] = groupBoxDoc019;
            _groupBoxArray[20] = groupBoxDoc020;
            _groupBoxArray[21] = groupBoxDoc021;
            _groupBoxArray[22] = groupBoxDoc022;
            _groupBoxArray[23] = groupBoxDoc023;
            _groupBoxArray[24] = groupBoxDoc024;
            _groupBoxArray[25] = groupBoxDoc025;
            _groupBoxArray[26] = groupBoxDoc026;
            _groupBoxArray[27] = groupBoxDoc027;
            _groupBoxArray[28] = groupBoxDoc028;
            _groupBoxArray[29] = groupBoxDoc029;
            _groupBoxArray[30] = groupBoxDoc030;
            _groupBoxArray[31] = groupBoxDoc031;
            _groupBoxArray[32] = groupBoxDoc032;
            _groupBoxArray[33] = groupBoxDoc033;
            _groupBoxArray[34] = groupBoxDoc034;
            _groupBoxArray[35] = groupBoxDoc035;
            _groupBoxArray[36] = groupBoxDoc036;
            _groupBoxArray[37] = groupBoxDoc037;
            _groupBoxArray[38] = groupBoxDoc038;
            _groupBoxArray[39] = groupBoxDoc039;
            _groupBoxArray[40] = groupBoxDoc040;
            _groupBoxArray[41] = groupBoxDoc041;
            _groupBoxArray[42] = groupBoxDoc042;
            _groupBoxArray[43] = groupBoxDoc043;
            _groupBoxArray[44] = groupBoxDoc044;
            _groupBoxArray[45] = groupBoxDoc045;
            _groupBoxArray[46] = groupBoxDev001;
            _groupBoxArray[47] = groupBoxDev002;
            _groupBoxArray[48] = groupBoxDev003;

            _labelWorkTimeArray = new Label[49];
            _labelWorkTimeArray[0] = null;
            _labelWorkTimeArray[1] = labelWorkTimeDoc001;
            _labelWorkTimeArray[2] = labelWorkTimeDoc002;
            _labelWorkTimeArray[3] = labelWorkTimeDoc003;
            _labelWorkTimeArray[4] = labelWorkTimeDoc004;
            _labelWorkTimeArray[5] = labelWorkTimeDoc005;
            _labelWorkTimeArray[6] = labelWorkTimeDoc006;
            _labelWorkTimeArray[7] = labelWorkTimeDoc007;
            _labelWorkTimeArray[8] = labelWorkTimeDoc008;
            _labelWorkTimeArray[9] = labelWorkTimeDoc009;
            _labelWorkTimeArray[10] = labelWorkTimeDoc010;
            _labelWorkTimeArray[11] = labelWorkTimeDoc011;
            _labelWorkTimeArray[12] = labelWorkTimeDoc012;
            _labelWorkTimeArray[13] = labelWorkTimeDoc013;
            _labelWorkTimeArray[14] = labelWorkTimeDoc014;
            _labelWorkTimeArray[15] = labelWorkTimeDoc015;
            _labelWorkTimeArray[16] = labelWorkTimeDoc016;
            _labelWorkTimeArray[17] = labelWorkTimeDoc017;
            _labelWorkTimeArray[18] = labelWorkTimeDoc018;
            _labelWorkTimeArray[19] = labelWorkTimeDoc019;
            _labelWorkTimeArray[20] = labelWorkTimeDoc020;
            _labelWorkTimeArray[21] = labelWorkTimeDoc021;
            _labelWorkTimeArray[22] = labelWorkTimeDoc022;
            _labelWorkTimeArray[23] = labelWorkTimeDoc023;
            _labelWorkTimeArray[24] = labelWorkTimeDoc024;
            _labelWorkTimeArray[25] = labelWorkTimeDoc025;
            _labelWorkTimeArray[26] = labelWorkTimeDoc026;
            _labelWorkTimeArray[27] = labelWorkTimeDoc027;
            _labelWorkTimeArray[28] = labelWorkTimeDoc028;
            _labelWorkTimeArray[29] = labelWorkTimeDoc029;
            _labelWorkTimeArray[30] = labelWorkTimeDoc030;
            _labelWorkTimeArray[31] = labelWorkTimeDoc031;
            _labelWorkTimeArray[32] = labelWorkTimeDoc032;
            _labelWorkTimeArray[33] = labelWorkTimeDoc033;
            _labelWorkTimeArray[34] = labelWorkTimeDoc034;
            _labelWorkTimeArray[35] = labelWorkTimeDoc035;
            _labelWorkTimeArray[36] = labelWorkTimeDoc036;
            _labelWorkTimeArray[37] = labelWorkTimeDoc037;
            _labelWorkTimeArray[38] = labelWorkTimeDoc038;
            _labelWorkTimeArray[39] = labelWorkTimeDoc039;
            _labelWorkTimeArray[40] = labelWorkTimeDoc040;
            _labelWorkTimeArray[41] = labelWorkTimeDoc041;
            _labelWorkTimeArray[42] = labelWorkTimeDoc042;
            _labelWorkTimeArray[43] = labelWorkTimeDoc043;
            _labelWorkTimeArray[44] = labelWorkTimeDoc044;
            _labelWorkTimeArray[45] = labelWorkTimeDoc045;
            _labelWorkTimeArray[46] = labelWorkTimeDev001;
            _labelWorkTimeArray[47] = labelWorkTimeDev002;
            _labelWorkTimeArray[48] = labelWorkTimeDev003;

            _labelPatientNowArray = new Label[49];
            _labelPatientNowArray[1] = labelPatientNowDoc001;
            _labelPatientNowArray[2] = labelPatientNowDoc002;
            _labelPatientNowArray[3] = labelPatientNowDoc003;
            _labelPatientNowArray[4] = labelPatientNowDoc004;
            _labelPatientNowArray[5] = labelPatientNowDoc005;
            _labelPatientNowArray[6] = labelPatientNowDoc006;
            _labelPatientNowArray[7] = labelPatientNowDoc007;
            _labelPatientNowArray[8] = labelPatientNowDoc008;
            _labelPatientNowArray[9] = labelPatientNowDoc009;
            _labelPatientNowArray[10] = labelPatientNowDoc010;
            _labelPatientNowArray[11] = labelPatientNowDoc011;
            _labelPatientNowArray[12] = labelPatientNowDoc012;
            _labelPatientNowArray[13] = labelPatientNowDoc013;
            _labelPatientNowArray[14] = labelPatientNowDoc014;
            _labelPatientNowArray[15] = labelPatientNowDoc015;
            _labelPatientNowArray[16] = labelPatientNowDoc016;
            _labelPatientNowArray[17] = labelPatientNowDoc017;
            _labelPatientNowArray[18] = labelPatientNowDoc018;
            _labelPatientNowArray[19] = labelPatientNowDoc019;
            _labelPatientNowArray[20] = labelPatientNowDoc020;
            _labelPatientNowArray[21] = labelPatientNowDoc021;
            _labelPatientNowArray[22] = labelPatientNowDoc022;
            _labelPatientNowArray[23] = labelPatientNowDoc023;
            _labelPatientNowArray[24] = labelPatientNowDoc024;
            _labelPatientNowArray[25] = labelPatientNowDoc025;
            _labelPatientNowArray[26] = labelPatientNowDoc026;
            _labelPatientNowArray[27] = labelPatientNowDoc027;
            _labelPatientNowArray[28] = labelPatientNowDoc028;
            _labelPatientNowArray[29] = labelPatientNowDoc029;
            _labelPatientNowArray[30] = labelPatientNowDoc030;
            _labelPatientNowArray[31] = labelPatientNowDoc031;
            _labelPatientNowArray[32] = labelPatientNowDoc032;
            _labelPatientNowArray[33] = labelPatientNowDoc033;
            _labelPatientNowArray[34] = labelPatientNowDoc034;
            _labelPatientNowArray[35] = labelPatientNowDoc035;
            _labelPatientNowArray[36] = labelPatientNowDoc036;
            _labelPatientNowArray[37] = labelPatientNowDoc037;
            _labelPatientNowArray[38] = labelPatientNowDoc038;
            _labelPatientNowArray[39] = labelPatientNowDoc039;
            _labelPatientNowArray[40] = labelPatientNowDoc040;
            _labelPatientNowArray[41] = labelPatientNowDoc041;
            _labelPatientNowArray[42] = labelPatientNowDoc042;
            _labelPatientNowArray[43] = labelPatientNowDoc043;
            _labelPatientNowArray[44] = labelPatientNowDoc044;
            _labelPatientNowArray[45] = labelPatientNowDoc045;
            _labelPatientNowArray[46] = labelPatientNowDev001;
            _labelPatientNowArray[47] = labelPatientNowDev002;
            _labelPatientNowArray[48] = labelPatientNowDev003;

            _listBoxPatientWaitArray = new ListBox[49];
            _listBoxPatientWaitArray[0] = null;
            _listBoxPatientWaitArray[1] = listBoxPatientWaitDoc001;
            _listBoxPatientWaitArray[2] = listBoxPatientWaitDoc002;
            _listBoxPatientWaitArray[3] = listBoxPatientWaitDoc003;
            _listBoxPatientWaitArray[4] = listBoxPatientWaitDoc004;
            _listBoxPatientWaitArray[5] = listBoxPatientWaitDoc005;
            _listBoxPatientWaitArray[6] = listBoxPatientWaitDoc006;
            _listBoxPatientWaitArray[7] = listBoxPatientWaitDoc007;
            _listBoxPatientWaitArray[8] = listBoxPatientWaitDoc008;
            _listBoxPatientWaitArray[9] = listBoxPatientWaitDoc009;
            _listBoxPatientWaitArray[10] = listBoxPatientWaitDoc010;
            _listBoxPatientWaitArray[11] = listBoxPatientWaitDoc011;
            _listBoxPatientWaitArray[12] = listBoxPatientWaitDoc012;
            _listBoxPatientWaitArray[13] = listBoxPatientWaitDoc013;
            _listBoxPatientWaitArray[14] = listBoxPatientWaitDoc014;
            _listBoxPatientWaitArray[15] = listBoxPatientWaitDoc015;
            _listBoxPatientWaitArray[16] = listBoxPatientWaitDoc016;
            _listBoxPatientWaitArray[17] = listBoxPatientWaitDoc017;
            _listBoxPatientWaitArray[18] = listBoxPatientWaitDoc018;
            _listBoxPatientWaitArray[19] = listBoxPatientWaitDoc019;
            _listBoxPatientWaitArray[20] = listBoxPatientWaitDoc020;
            _listBoxPatientWaitArray[21] = listBoxPatientWaitDoc021;
            _listBoxPatientWaitArray[22] = listBoxPatientWaitDoc022;
            _listBoxPatientWaitArray[23] = listBoxPatientWaitDoc023;
            _listBoxPatientWaitArray[24] = listBoxPatientWaitDoc024;
            _listBoxPatientWaitArray[25] = listBoxPatientWaitDoc025;
            _listBoxPatientWaitArray[26] = listBoxPatientWaitDoc026;
            _listBoxPatientWaitArray[27] = listBoxPatientWaitDoc027;
            _listBoxPatientWaitArray[28] = listBoxPatientWaitDoc028;
            _listBoxPatientWaitArray[29] = listBoxPatientWaitDoc029;
            _listBoxPatientWaitArray[30] = listBoxPatientWaitDoc030;
            _listBoxPatientWaitArray[31] = listBoxPatientWaitDoc031;
            _listBoxPatientWaitArray[32] = listBoxPatientWaitDoc032;
            _listBoxPatientWaitArray[33] = listBoxPatientWaitDoc033;
            _listBoxPatientWaitArray[34] = listBoxPatientWaitDoc034;
            _listBoxPatientWaitArray[35] = listBoxPatientWaitDoc035;
            _listBoxPatientWaitArray[36] = listBoxPatientWaitDoc036;
            _listBoxPatientWaitArray[37] = listBoxPatientWaitDoc037;
            _listBoxPatientWaitArray[38] = listBoxPatientWaitDoc038;
            _listBoxPatientWaitArray[39] = listBoxPatientWaitDoc039;
            _listBoxPatientWaitArray[40] = listBoxPatientWaitDoc040;
            _listBoxPatientWaitArray[41] = listBoxPatientWaitDoc041;
            _listBoxPatientWaitArray[42] = listBoxPatientWaitDoc042;
            _listBoxPatientWaitArray[43] = listBoxPatientWaitDoc043;
            _listBoxPatientWaitArray[44] = listBoxPatientWaitDoc044;
            _listBoxPatientWaitArray[45] = listBoxPatientWaitDoc045;
            _listBoxPatientWaitArray[46] = listBoxPatientWaitDev001;
            _listBoxPatientWaitArray[47] = listBoxPatientWaitDev002;
            _listBoxPatientWaitArray[48] = listBoxPatientWaitDev003;

            _listBoxAfterCheckArray = new ListBox[46];
            _listBoxAfterCheckArray[0] = null;
            _listBoxAfterCheckArray[1] = listBoxAfterCheckDoc001;
            _listBoxAfterCheckArray[2] = listBoxAfterCheckDoc002;
            _listBoxAfterCheckArray[3] = listBoxAfterCheckDoc003;
            _listBoxAfterCheckArray[4] = listBoxAfterCheckDoc004;
            _listBoxAfterCheckArray[5] = listBoxAfterCheckDoc005;
            _listBoxAfterCheckArray[6] = listBoxAfterCheckDoc006;
            _listBoxAfterCheckArray[7] = listBoxAfterCheckDoc007;
            _listBoxAfterCheckArray[8] = listBoxAfterCheckDoc008;
            _listBoxAfterCheckArray[9] = listBoxAfterCheckDoc009;
            _listBoxAfterCheckArray[10] = listBoxAfterCheckDoc010;
            _listBoxAfterCheckArray[11] = listBoxAfterCheckDoc011;
            _listBoxAfterCheckArray[12] = listBoxAfterCheckDoc012;
            _listBoxAfterCheckArray[13] = listBoxAfterCheckDoc013;
            _listBoxAfterCheckArray[14] = listBoxAfterCheckDoc014;
            _listBoxAfterCheckArray[15] = listBoxAfterCheckDoc015;
            _listBoxAfterCheckArray[16] = listBoxAfterCheckDoc016;
            _listBoxAfterCheckArray[17] = listBoxAfterCheckDoc017;
            _listBoxAfterCheckArray[18] = listBoxAfterCheckDoc018;
            _listBoxAfterCheckArray[19] = listBoxAfterCheckDoc019;
            _listBoxAfterCheckArray[20] = listBoxAfterCheckDoc020;
            _listBoxAfterCheckArray[21] = listBoxAfterCheckDoc021;
            _listBoxAfterCheckArray[22] = listBoxAfterCheckDoc022;
            _listBoxAfterCheckArray[23] = listBoxAfterCheckDoc023;
            _listBoxAfterCheckArray[24] = listBoxAfterCheckDoc024;
            _listBoxAfterCheckArray[25] = listBoxAfterCheckDoc025;
            _listBoxAfterCheckArray[26] = listBoxAfterCheckDoc026;
            _listBoxAfterCheckArray[27] = listBoxAfterCheckDoc027;
            _listBoxAfterCheckArray[28] = listBoxAfterCheckDoc028;
            _listBoxAfterCheckArray[29] = listBoxAfterCheckDoc029;
            _listBoxAfterCheckArray[30] = listBoxAfterCheckDoc030;
            _listBoxAfterCheckArray[31] = listBoxAfterCheckDoc031;
            _listBoxAfterCheckArray[32] = listBoxAfterCheckDoc032;
            _listBoxAfterCheckArray[33] = listBoxAfterCheckDoc033;
            _listBoxAfterCheckArray[34] = listBoxAfterCheckDoc034;
            _listBoxAfterCheckArray[35] = listBoxAfterCheckDoc035;
            _listBoxAfterCheckArray[36] = listBoxAfterCheckDoc036;
            _listBoxAfterCheckArray[37] = listBoxAfterCheckDoc037;
            _listBoxAfterCheckArray[38] = listBoxAfterCheckDoc038;
            _listBoxAfterCheckArray[39] = listBoxAfterCheckDoc039;
            _listBoxAfterCheckArray[40] = listBoxAfterCheckDoc040;
            _listBoxAfterCheckArray[41] = listBoxAfterCheckDoc041;
            _listBoxAfterCheckArray[42] = listBoxAfterCheckDoc042;
            _listBoxAfterCheckArray[43] = listBoxAfterCheckDoc043;
            _listBoxAfterCheckArray[44] = listBoxAfterCheckDoc044;
            _listBoxAfterCheckArray[45] = listBoxAfterCheckDoc045;

            //初始化其他

            _firstRefresh = true;
            _random = new Random();
            _time = 0;
            _allFinish = false;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            SetButtonInput(true);
            SetButtonOutput(false);
            SetButtonStart(false);
            buttonReset.Visible = false;
            buttonOutput.Visible = false;
            label191.Visible = false;
            listBoxRegistration.Visible = false;
            tabControlDepartment.Visible = false;
            groupBoxTimeAdd.Visible = false;
            listBoxTimeAddNumber.SelectedIndex = 0;
        }

        private void buttonInput_Click(object sender, EventArgs e)
        {
            SetButtonInput(false);
            this.Enabled = false;
            _formInput = new FormInput(this);
            _formInput.Show();
        }

        private void buttonOutput_Click(object sender, EventArgs e)
        {
            SetButtonOutput(false);
            this.Enabled = false;
            _formOutput = new FormOutput(this);
            _formOutput.Show();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            SetButtonStart(false);
            //更新窗体呈现的信息
            refreshForm();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            groupBoxTimeAdd.Enabled = true;
            SetButtonOutput(false);
            _time = 0;
            _patientPointer = 0;
            for (Int32 i = 1; i <= _patientNumber; i++)
            {
                _patient[i].SetDoctor(null);
                _patient[i].ResetDoctorTime();
                _patient[i].SetDeviceDepartment(null);
                _patient[i].SetDevice(null);
                _patient[i].ResetDeviceTime();
            }
            for (Int32 i = 1; i <= 45; i++)
            {
                _doctor[i].ResetWorkTime();
                _doctor[i].SetPatientNow(null);
                _doctor[i].GetPatientWait().clear();
                _doctor[i].GetAfterCheck().clear();
                _doctor[i].SetSwitchTime(0);
                _doctor[i].SetSwitch(true);
            }
            for (Int32 i = 1; i <= 3; i++)
            {
                _device[i].ResetWorkTime();
                _device[i].SetPatientNow(null);
                _device[i].GetPatientWait().clear();
                _device[i].SetSwitchTime(0);
                _device[i].SetSwitch(true);
            }
            //更新窗体呈现的信息
            refreshForm();
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            _formThanks = new FormThanks(this);
            _formThanks.Show();
        }

        private void buttonTimeAdd_Click(object sender, EventArgs e)
        {
            Int32 timeAdd = Convert.ToInt32(listBoxTimeAddNumber.SelectedItem.ToString());
            Patient pat = null;
            DoctorDepartment docDep = null;
            DeviceDepartment devDep = null;
            Doctor doc = null;
            Device dev = null;
            Int32 temp = 0;

            for (Int32 i = 1; i <= timeAdd; i++)
            {
                //递增全局时间
                if (_time <= 65536)
                {
                    _time++;
                }
                else
                {
                    groupBoxTimeAdd.Enabled = false;
                    return;
                }
                //挂号患者依此挂号
                //患者挂号之后立即分配医生，并进入该医生等待就诊的患者队列
                //【间隔1个时间单位】
                for (Int32 j = 1; j <= 50; j++)
                {
                    if (_patientPointer < _patientNumber)
                    {
                        _patientPointer++;
                        pat = _patient[_patientPointer];
                    }
                    else
                    {
                        pat = null;
                    }
                    if (pat != null)
                    {
                        //为患者分配医生
                        docDep = pat.GetDoctorDepartment();
                        docDep.ChangeDistribution();
                        doc = (docDep.GetDoctor())[docDep.GetDistribution()];
                        pat.SetDoctor(doc);
                        //将患者放入该医生的等待就诊的患者队列
                        doc.GetPatientWait().enterElem(pat);
                    }
                }
                //医生从患者队列中取出队头患者问诊
                //优先选取检查完成后返回的患者队列中的患者
                //【医生对每个患者的问诊时间为2-9个时间单位】
                for (Int32 j = 1; j <= 45; j++)
                {
                    doc = _doctor[j];
                    if (doc.GetSwitch())    //该医生需要叫号（下一位患者）
                    {
                        doc.SetSwitch(false);
                        //首先尝试从检查完成后返回的患者队列中取出患者
                        if (doc.GetAfterCheck().quitElem(ref pat) != SequentialQueue<Patient>.Infeasible)
                        {
                            doc.SetPatientNow(pat);
                            //设定切换时间
                            doc.SetSwitchTime(_time + timeDoctor());
                        }
                        else
                        {
                            //然后尝试从等待就诊的患者队列中取出患者
                            if (doc.GetPatientWait().quitElem(ref pat) != SequentialQueue<Patient>.Infeasible)
                            {
                                doc.SetPatientNow(pat);
                                //设定切换时间
                                temp = timeDoctor();
                                doc.SetSwitchTime(_time + temp);
                                //指定部分患者做B超检查
                                if (temp <= 4)
                                {
                                    pat.SetNeedCheck(true);
                                }
                            }
                            else
                            {
                                //如果都不行，只好设置为null
                                doc.SetPatientNow(null);
                                doc.SetSwitch(true);
                            }
                        }
                    }
                    else    //该医生正在问诊
                    {
                        //递增医生的工作时间
                        doc.PlusWorkTime();
                        //递增该医生当前正在问诊的患者的就诊时间
                        pat = doc.GetPatientNow();
                        pat.PlusDoctorTime();
                        //问诊时间满，需要切换
                        if (_time >= doc.GetSwitchTime())
                        {
                            //如果患者需要做检查
                            if (pat.GetNeedCheck())
                            {
                                //为患者分配设备（B超机）
                                devDep = _deviceDepartment[1];
                                devDep.ChangeDistribution();
                                dev = (devDep.GetDevice())[devDep.GetDistribution()];
                                pat.SetDeviceDepartment(devDep);
                                pat.SetDevice(dev);
                                //将患者放入该B超机等待就诊的患者队列
                                dev.GetPatientWait().enterElem(pat);
                                pat.SetNeedCheck(false);
                            }
                            //切换
                            doc.SetSwitch(true);
                        }
                    }
                }
                //设备从等待检查的患者队列中取出队头患者检查
                //【设备对每个患者的检查时间为4-7个时间单位】
                for (Int32 j = 1; j <= 3; j++)
                {
                    dev = _device[j];
                    if (dev.GetSwitch())    //该设备需要叫号（下一位患者）
                    {
                        dev.SetSwitch(false);
                        //尝试从等待检查的患者队列中取出患者
                        if (dev.GetPatientWait().quitElem(ref pat) != SequentialQueue<Patient>.Infeasible)
                        {
                            dev.SetPatientNow(pat);
                            //设定切换时间
                            dev.SetSwitchTime(_time + timeDevice());
                        }
                        else
                        {
                            //如果不行，只好设置为null
                            dev.SetPatientNow(null);
                            dev.SetSwitch(true);
                        }
                    }
                    else    //该设备正在检查
                    {
                        //递增设备的工作时间
                        dev.PlusWorkTime();
                        //递增该设备当前正在检查的患者的就诊时间
                        pat = dev.GetPatientNow();
                        pat.PlusDeviceTime();
                        //检查时间满，需要切换
                        if (_time >= dev.GetSwitchTime())
                        {
                            doc = pat.GetDoctor();
                            doc.GetAfterCheck().enterElem(pat);
                            //切换
                            dev.SetSwitch(true);
                        }
                    }
                }
            }
            //更新窗体呈现的信息
            refreshForm();
            //判定整个模拟是否已经全部完成
            if (listBoxRegistration.Items.Count != 0)
            {
                _allFinish = false;
            }
            else
            {
                _allFinish = true;
                for (Int32 i = 1; i <= 3; i++)
                {
                    if (i == 1)
                    {
                        for (Int32 j = 1; j <= 48; j++)
                        {
                            if (_labelPatientNowArray[j].Text != "无")
                            {
                                _allFinish = false;
                                break;
                            }
                        }
                        if (_allFinish == false)
                        {
                            break;
                        }
                    }
                    else if (i == 2)
                    {

                        for (Int32 j = 1; j <= 48; j++)
                        {
                            if (_listBoxPatientWaitArray[i].Items.Count != 0)
                            {
                                _allFinish = false;
                                break;
                            }
                        }
                        if (_allFinish == false)
                        {
                            break;
                        }
                    }
                    else
                    {
                        for (Int32 j = 1; j <= 48; j++)
                        {
                            if (_listBoxAfterCheckArray[i].Items.Count != 0)
                            {
                                _allFinish = false;
                                break;
                            }
                        }
                        if (_allFinish == false)
                        {
                            break;
                        }
                    }
                }
            }
            if (_allFinish)
            {
                groupBoxTimeAdd.Enabled = false;
                SetButtonOutput(true);
            }
        }

        private void listBoxRegistration_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxRegistration.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxRegistration.SelectedItem.ToString());
        }

        //button

        private void buttonDep1001_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_doctor[1].ReceiveStaticInformation() + "\r\n" + _doctor[2].ReceiveStaticInformation() + "\r\n" + _doctor[3].ReceiveStaticInformation(), "医生信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDep1002_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_doctor[4].ReceiveStaticInformation() + "\r\n" + _doctor[5].ReceiveStaticInformation() + "\r\n" + _doctor[6].ReceiveStaticInformation(), "医生信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDep1003_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_doctor[7].ReceiveStaticInformation() + "\r\n" + _doctor[8].ReceiveStaticInformation() + "\r\n" + _doctor[9].ReceiveStaticInformation(), "医生信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDep1004_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_doctor[10].ReceiveStaticInformation() + "\r\n" + _doctor[11].ReceiveStaticInformation() + "\r\n" + _doctor[12].ReceiveStaticInformation(), "医生信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDep1005_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_doctor[13].ReceiveStaticInformation() + "\r\n" + _doctor[14].ReceiveStaticInformation() + "\r\n" + _doctor[15].ReceiveStaticInformation(), "医生信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDep1006_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_doctor[16].ReceiveStaticInformation() + "\r\n" + _doctor[17].ReceiveStaticInformation() + "\r\n" + _doctor[18].ReceiveStaticInformation(), "医生信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDep1007_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_doctor[19].ReceiveStaticInformation() + "\r\n" + _doctor[20].ReceiveStaticInformation() + "\r\n" + _doctor[21].ReceiveStaticInformation(), "医生信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDep1008_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_doctor[22].ReceiveStaticInformation() + "\r\n" + _doctor[23].ReceiveStaticInformation() + "\r\n" + _doctor[24].ReceiveStaticInformation(), "医生信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDep1009_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_doctor[25].ReceiveStaticInformation() + "\r\n" + _doctor[26].ReceiveStaticInformation() + "\r\n" + _doctor[27].ReceiveStaticInformation(), "医生信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDep1010_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_doctor[28].ReceiveStaticInformation() + "\r\n" + _doctor[29].ReceiveStaticInformation() + "\r\n" + _doctor[30].ReceiveStaticInformation(), "医生信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDep1011_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_doctor[31].ReceiveStaticInformation() + "\r\n" + _doctor[32].ReceiveStaticInformation() + "\r\n" + _doctor[33].ReceiveStaticInformation(), "医生信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDep1012_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_doctor[34].ReceiveStaticInformation() + "\r\n" + _doctor[35].ReceiveStaticInformation() + "\r\n" + _doctor[36].ReceiveStaticInformation(), "医生信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDep1013_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_doctor[37].ReceiveStaticInformation() + "\r\n" + _doctor[38].ReceiveStaticInformation() + "\r\n" + _doctor[39].ReceiveStaticInformation(), "医生信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDep1014_Click(object sender, EventArgs e)
        {
             MessageBox.Show(_doctor[40].ReceiveStaticInformation() + "\r\n" + _doctor[41].ReceiveStaticInformation() + "\r\n" + _doctor[42].ReceiveStaticInformation(), "医生信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDep1015_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_doctor[43].ReceiveStaticInformation() + "\r\n" + _doctor[44].ReceiveStaticInformation() + "\r\n" + _doctor[45].ReceiveStaticInformation(), "医生信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDep2001_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_device[1].ReceiveStaticInformation() + "\r\n" + _device[2].ReceiveStaticInformation() + "\r\n" + _device[3].ReceiveStaticInformation(), "医生信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //labelPatientNow

        private void labelPatientNowDoc001_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc001.Text == null || labelPatientNowDoc001.Text == "" || labelPatientNowDoc001.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc001.Text);
        }

        private void labelPatientNowDoc002_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc002.Text == null || labelPatientNowDoc002.Text == "" || labelPatientNowDoc002.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc002.Text);
        }

        private void labelPatientNowDoc003_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc003.Text == null || labelPatientNowDoc003.Text == "" || labelPatientNowDoc003.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc003.Text);
        }

        private void labelPatientNowDoc004_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc004.Text == null || labelPatientNowDoc004.Text == "" || labelPatientNowDoc004.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc004.Text);
        }

        private void labelPatientNowDoc005_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc005.Text == null || labelPatientNowDoc005.Text == "" || labelPatientNowDoc005.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc005.Text);
        }

        private void labelPatientNowDoc006_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc006.Text == null || labelPatientNowDoc006.Text == "" || labelPatientNowDoc006.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc006.Text);
        }

        private void labelPatientNowDoc007_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc007.Text == null || labelPatientNowDoc007.Text == "" || labelPatientNowDoc007.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc007.Text);
        }

        private void labelPatientNowDoc008_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc008.Text == null || labelPatientNowDoc008.Text == "" || labelPatientNowDoc008.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc008.Text);
        }

        private void labelPatientNowDoc009_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc009.Text == null || labelPatientNowDoc009.Text == "" || labelPatientNowDoc009.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc009.Text);
        }

        private void labelPatientNowDoc010_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc010.Text == null || labelPatientNowDoc010.Text == "" || labelPatientNowDoc010.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc010.Text);
        }

        private void labelPatientNowDoc011_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc011.Text == null || labelPatientNowDoc011.Text == "" || labelPatientNowDoc011.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc011.Text);
        }

        private void labelPatientNowDoc012_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc012.Text == null || labelPatientNowDoc012.Text == "" || labelPatientNowDoc012.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc012.Text);
        }

        private void labelPatientNowDoc013_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc013.Text == null || labelPatientNowDoc013.Text == "" || labelPatientNowDoc013.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc013.Text);
        }

        private void labelPatientNowDoc014_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc014.Text == null || labelPatientNowDoc014.Text == "" || labelPatientNowDoc014.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc014.Text);
        }

        private void labelPatientNowDoc015_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc015.Text == null || labelPatientNowDoc015.Text == "" || labelPatientNowDoc015.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc015.Text);
        }

        private void labelPatientNowDoc016_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc016.Text == null || labelPatientNowDoc016.Text == "" || labelPatientNowDoc016.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc016.Text);
        }

        private void labelPatientNowDoc017_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc017.Text == null || labelPatientNowDoc017.Text == "" || labelPatientNowDoc017.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc017.Text);
        }

        private void labelPatientNowDoc018_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc018.Text == null || labelPatientNowDoc018.Text == "" || labelPatientNowDoc018.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc018.Text);
        }

        private void labelPatientNowDoc019_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc019.Text == null || labelPatientNowDoc019.Text == "" || labelPatientNowDoc019.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc019.Text);
        }

        private void labelPatientNowDoc020_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc020.Text == null || labelPatientNowDoc020.Text == "" || labelPatientNowDoc020.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc020.Text);
        }

        private void labelPatientNowDoc021_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc021.Text == null || labelPatientNowDoc021.Text == "" || labelPatientNowDoc021.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc021.Text);
        }

        private void labelPatientNowDoc022_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc022.Text == null || labelPatientNowDoc022.Text == "" || labelPatientNowDoc022.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc022.Text);
        }

        private void labelPatientNowDoc023_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc023.Text == null || labelPatientNowDoc023.Text == "" || labelPatientNowDoc023.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc023.Text);
        }

        private void labelPatientNowDoc024_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc024.Text == null || labelPatientNowDoc024.Text == "" || labelPatientNowDoc024.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc024.Text);
        }

        private void labelPatientNowDoc025_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc025.Text == null || labelPatientNowDoc025.Text == "" || labelPatientNowDoc025.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc025.Text);
        }

        private void labelPatientNowDoc026_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc026.Text == null || labelPatientNowDoc026.Text == "" || labelPatientNowDoc026.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc026.Text);
        }

        private void labelPatientNowDoc027_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc027.Text == null || labelPatientNowDoc027.Text == "" || labelPatientNowDoc027.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc027.Text);
        }

        private void labelPatientNowDoc028_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc028.Text == null || labelPatientNowDoc028.Text == "" || labelPatientNowDoc028.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc028.Text);
        }

        private void labelPatientNowDoc029_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc029.Text == null || labelPatientNowDoc029.Text == "" || labelPatientNowDoc029.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc029.Text);
        }

        private void labelPatientNowDoc030_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc030.Text == null || labelPatientNowDoc030.Text == "" || labelPatientNowDoc030.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc030.Text);
        }

        private void labelPatientNowDoc031_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc031.Text == null || labelPatientNowDoc031.Text == "" || labelPatientNowDoc031.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc031.Text);
        }

        private void labelPatientNowDoc032_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc032.Text == null || labelPatientNowDoc032.Text == "" || labelPatientNowDoc032.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc032.Text);
        }

        private void labelPatientNowDoc033_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc033.Text == null || labelPatientNowDoc033.Text == "" || labelPatientNowDoc033.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc033.Text);
        }

        private void labelPatientNowDoc034_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc034.Text == null || labelPatientNowDoc034.Text == "" || labelPatientNowDoc034.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc034.Text);
        }

        private void labelPatientNowDoc035_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc035.Text == null || labelPatientNowDoc035.Text == "" || labelPatientNowDoc035.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc035.Text);
        }

        private void labelPatientNowDoc036_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc036.Text == null || labelPatientNowDoc036.Text == "" || labelPatientNowDoc036.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc036.Text);
        }

        private void labelPatientNowDoc037_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc037.Text == null || labelPatientNowDoc037.Text == "" || labelPatientNowDoc037.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc037.Text);
        }

        private void labelPatientNowDoc038_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc038.Text == null || labelPatientNowDoc038.Text == "" || labelPatientNowDoc038.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc038.Text);
        }

        private void labelPatientNowDoc039_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc039.Text == null || labelPatientNowDoc039.Text == "" || labelPatientNowDoc039.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc039.Text);
        }

        private void labelPatientNowDoc040_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc040.Text == null || labelPatientNowDoc040.Text == "" || labelPatientNowDoc040.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc040.Text);
        }

        private void labelPatientNowDoc041_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc041.Text == null || labelPatientNowDoc041.Text == "" || labelPatientNowDoc041.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc041.Text);
        }

        private void labelPatientNowDoc042_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc042.Text == null || labelPatientNowDoc042.Text == "" || labelPatientNowDoc042.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc042.Text);
        }

        private void labelPatientNowDoc043_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc043.Text == null || labelPatientNowDoc043.Text == "" || labelPatientNowDoc043.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc043.Text);
        }

        private void labelPatientNowDoc044_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc044.Text == null || labelPatientNowDoc044.Text == "" || labelPatientNowDoc044.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc044.Text);
        }

        private void labelPatientNowDoc045_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDoc045.Text == null || labelPatientNowDoc045.Text == "" || labelPatientNowDoc045.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDoc045.Text);
        }

        private void labelPatientNowDev001_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDev001.Text == null || labelPatientNowDev001.Text == "" || labelPatientNowDev001.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDev001.Text);
        }

        private void labelPatientNowDev002_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDev002.Text == null || labelPatientNowDev002.Text == "" || labelPatientNowDev002.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDev002.Text);
        }

        private void labelPatientNowDev003_DoubleClick(object sender, EventArgs e)
        {
            if (labelPatientNowDev003.Text == null || labelPatientNowDev003.Text == "" || labelPatientNowDev003.Text == "无")
            {
                return;
            }
            displayPatientMessage(labelPatientNowDev003.Text);
        }

        //listBoxPatientWait

        private void listBoxPatientWaitDoc001_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc001.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc001.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc002_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc002.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc002.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc003_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc003.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc003.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc004_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc004.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc004.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc005_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc005.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc005.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc006_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc006.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc006.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc007_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc007.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc007.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc008_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc008.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc008.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc009_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc009.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc009.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc010_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc010.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc010.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc011_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc011.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc011.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc012_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc012.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc012.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc013_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc013.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc013.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc014_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc014.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc014.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc015_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc015.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc015.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc016_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc016.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc016.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc017_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc017.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc017.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc018_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc018.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc018.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc019_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc019.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc019.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc020_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc020.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc020.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc021_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc021.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc021.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc022_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc022.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc022.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc023_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc023.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc023.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc024_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc024.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc024.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc025_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc025.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc025.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc026_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc026.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc026.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc027_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc027.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc027.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc028_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc028.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc028.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc029_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc029.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc029.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc030_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc030.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc030.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc031_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc031.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc031.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc032_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc032.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc032.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc033_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc033.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc033.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc034_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc034.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc034.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc035_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc035.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc035.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc036_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc036.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc036.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc037_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc037.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc037.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc038_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc038.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc038.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc039_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc039.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc039.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc040_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc040.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc040.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc041_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc041.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc041.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc042_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc042.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc042.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc043_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc043.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc043.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc044_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc044.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc044.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDoc045_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDoc045.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDoc045.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDev001_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDev001.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDev001.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDev002_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDev002.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDev002.SelectedItem.ToString());
        }

        private void listBoxPatientWaitDev003_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPatientWaitDev003.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxPatientWaitDev003.SelectedItem.ToString());
        }

        //listBoxAfterCheck

        private void listBoxAfterCheckDoc001_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc001.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc001.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc002_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc002.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc002.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc003_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc003.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc003.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc004_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc004.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc004.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc005_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc005.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc005.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc006_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc006.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc006.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc007_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc007.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc007.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc008_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc008.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc008.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc009_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc009.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc009.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc010_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc010.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc010.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc011_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc011.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc011.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc012_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc012.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc012.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc013_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc013.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc013.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc014_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc014.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc014.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc015_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc015.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc015.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc016_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc016.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc016.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc017_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc017.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc017.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc018_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc018.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc018.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc019_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc019.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc019.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc020_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc020.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc020.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc021_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc021.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc021.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc022_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc022.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc022.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc023_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc023.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc023.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc024_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc024.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc024.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc025_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc025.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc025.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc026_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc026.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc026.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc027_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc027.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc027.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc028_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc028.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc028.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc029_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc029.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc029.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc030_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc030.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc030.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc031_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc031.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc031.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc032_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc032.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc032.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc033_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc033.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc033.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc034_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc034.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc034.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc035_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc035.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc035.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc036_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc036.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc036.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc037_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc037.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc037.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc038_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc038.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc038.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc039_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc039.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc039.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc040_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc040.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc040.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc041_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc041.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc041.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc042_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc042.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc042.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc043_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc043.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc043.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc044_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc044.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc044.SelectedItem.ToString());
        }

        private void listBoxAfterCheckDoc045_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAfterCheckDoc045.SelectedItem == null)
            {
                return;
            }
            displayPatientMessage(listBoxAfterCheckDoc045.SelectedItem.ToString());
        }
    }
}
