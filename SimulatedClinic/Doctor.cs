﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimulatedClinic
{
    class Doctor
    {
        /*      对象：字段      */

        String _id;                             //医生编号
        String _name;                           //医生姓名
        DoctorDepartment _department;           //医生所属科室
        String _other;                          //医生其他信息
        Patient _patientNow;                    //该医生当前正在问诊的患者
        SequentialQueue<Patient> _patientWait;  //该医生等待就诊的患者队列
        SequentialQueue<Patient> _afterCheck;   //检查完成后返回的患者队列（优先处理）
        Int32 _workTime;                        //医生工作时间
        Int32 _switchTime;                      //用于判断何时该切换到下一位患者（用于模拟过程）
        Boolean _switch;                        //如果为true，那么切换到下一位患者（用于模拟过程）

        /*      对象：构造与析构方法      */

        //构造方法（4个参数）
        public Doctor(String id, String name, DoctorDepartment department, String other)
        {
            SetId(id);
            SetName(name);
            SetDepartment(department);
            SetOther(other);
            _patientNow = null;
            _patientWait = new SequentialQueue<Patient>(64);
            _afterCheck = new SequentialQueue<Patient>(64);
            _workTime = 0;
            SetSwitchTime(0);
            SetSwitch(true);
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

        public DoctorDepartment GetDepartment()
        {
            return _department;
        }

        public String GetOther()
        {
            return _other;
        }

        public Patient GetPatientNow()
        {
            return _patientNow;
        }

        public SequentialQueue<Patient> GetPatientWait()
        {
            return _patientWait;
        }

        public SequentialQueue<Patient> GetAfterCheck()
        {
            return _afterCheck;
        }

        public Int32 GetWorkTime()
        {
            return _workTime;
        }

        public Int32 GetSwitchTime()
        {
            return _switchTime;
        }

        public Boolean GetSwitch()
        {
            return _switch;
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

        public void SetDepartment(DoctorDepartment department)
        {
            _department = department;
        }

        public void SetOther(String other)
        {
            _other = other;
        }

        public void SetPatientNow(Patient patientNow)
        {
            _patientNow = patientNow;
        }

        public void SetSwitchTime(Int32 switchTime)
        {
            _switchTime = switchTime;
        }

        public void SetSwitch(Boolean s)
        {
            _switch = s;
        }

        //WorkTime增加一个时间单位
        public void PlusWorkTime()
        {
            _workTime += 1;
        }

        //重置_workTime
        public void ResetWorkTime()
        {
            _workTime = 0;
        }

        //获取静态信息
        public string ReceiveStaticInformation()
        {
            String result;
            result = "编号：" + _id + "\r\n";
            result += "姓名：" + _name + "\r\n";
            result += "所属科室：" + _department.GetId().ToString() + " " + _department.GetName() + "\r\n";
            result += "其他信息：" + _other + "\r\n";
            return result;
        }
    }
}
