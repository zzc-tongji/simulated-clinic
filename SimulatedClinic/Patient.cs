using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimulatedClinic
{
    class Patient
    {
        /*      对象：字段      */

        //输入信息
        String _id;                             //患者编号
        String _name;                           //患者姓名
        Sex _sex;                               //患者性别
        Int32 _age;                             //患者年龄
        String _other;                          //患者其他信息
        DoctorDepartment _doctorDepartment;     //患者就诊科室

        //记录信息
        Doctor _doctor;                         //为该患者诊疗的医生
        Int32 _doctorTime;                      //医生问诊时间
        DeviceDepartment _deviceDepartment;     //患者检测科室（如未做检测，设为null）
        Device _device;                         //为该患者做检测的设备（如未做检测，设为null）
        Int32 _deviceTime;                      //设备检查时间

        //标志信息
        Boolean _needCheck;              //该患者是否需要做设备（B超）检查

        /*      对象：构造与析构方法      */

        //构造方法(5个参数)
        public Patient(String id, String name, Sex sex, Int32 age, String other, DoctorDepartment department)
        {
            SetId(id);
            SetName(name);
            SetSex(sex);
            SetAge(age);
            SetOther(other);
            SetDoctorDepartment(department);
            SetDoctor();
            _doctorTime = 0;
            SetDeviceDepartment();
            SetDevice();
            _deviceTime = 0;
            SetNeedCheck(false);
        }

        /*      对象：功能方法      */

        //Get方法系列

        public String GetId()
        {
            return _id;
        }

        public String GetName()
        {
            return _name;
        }

        public Sex GetSex()
        {
            return _sex;
        }

        public Int32 GetAge()
        {
            return _age;
        }

        public String GetOther()
        {
            return _other;
        }

        public DoctorDepartment GetDoctorDepartment()
        {
            return _doctorDepartment;
        }

        public Doctor GetDoctor()
        {
            return _doctor;
        }

        public Int32 GetDoctorTime()
        {
            return _doctorTime;
        }

        public DeviceDepartment GetDeviceDepartment()
        {
            return _deviceDepartment;
        }

        public Device GetDevice()
        {
            return _device;
        }

        public Int32 GetDeviceTime()
        {
            return _deviceTime;
        }

        public Boolean GetNeedCheck()
        {
            return _needCheck;
        }

        //Set方法系列

        public void SetId(String id)
        {
            _id = id;
        }

        public void SetName(String name)
        {
            _name = name;
        }

        public void SetSex(Sex sex)
        {
            _sex = sex;
        }

        public void SetAge(Int32 age)
        {
            _age = age;
        }

        public void SetOther(String other)
        {
            _other = other;
        }

        public void SetDoctorDepartment(DoctorDepartment doctorDepartment = null)
        {
            _doctorDepartment = doctorDepartment;
        }

        public void SetDoctor(Doctor doctor = null)
        {
            _doctor = doctor;
        }

        public void SetDeviceDepartment(DeviceDepartment deviceDepartment = null)
        {
            _deviceDepartment = deviceDepartment;
        }

        public void SetDevice(Device device = null)
        {
            _device = device;
        }

        public void SetNeedCheck(Boolean needCheck)
        {
            _needCheck = needCheck;
        }

        //为_doctorTime增加一个时间单位
        public void PlusDoctorTime()
        {
            _doctorTime += 1;
        }

        //为_deviceTime增加一个时间单位
        public void PlusDeviceTime()
        {
            _deviceTime += 1;
        }

        //重置_doctorTime
        public void ResetDoctorTime()
        {
            _doctorTime = 0;
        }

        //重置_deviceTime
        public void ResetDeviceTime()
        {
            _deviceTime = 0;
        }

        //获取信息
        public string ReceiveInformation()
        {
            String result;
            result = "编号：" + _id + "\r\n";
            result += "姓名：" + _name + "\r\n";
            result += "性别：";
            if (_sex == Sex.Male)
            {
                result += "男";
            }
            else if (_sex == Sex.Female)
            {
                result += "女";
            }
            else if (_sex == Sex.Other)
            {
                result += "其他";
            }
            else
            {
                result += "";
            }
            result += "\r\n";
            result += "年龄：" + _age.ToString() + "\r\n";
            result += "其他信息：" + _other + "\r\n";
            result += "\r\n";
            if ( _doctorDepartment != null)
            {
                result += "就诊科室：" + _doctorDepartment.GetId().ToString() + " " + _doctorDepartment.GetName() + "\r\n";
            }
            if (_doctor != null)
            {
                result += "问诊医生：" + _doctor.GetId().ToString() + " " + _doctor.GetName() + "\r\n";
            }
            if (_doctorTime != 0)
            {
                result += "医生问诊时间：" + _doctorTime.ToString() + "\r\n";
            }
            if (_deviceDepartment != null)
            {
                result += "检查科室：" + _deviceDepartment.GetId().ToString() + " " + _deviceDepartment.GetName() + "\r\n";
                result += "检查设备：" + _device.GetId().ToString() + " " + _device.GetName() + "\r\n";
                result += "设备检查时间：" + _deviceTime.ToString() + "\r\n";
            }
            return result;
        }
    }
}
