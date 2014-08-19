using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimulatedClinic
{
    class DeviceDepartment
    {
        /*      对象：字段      */

        String _id;                             //科室编号
        String _name;                           //科室名称
        Device[] _device;                       //设备
        Int32 _distribution;                    //用于分配患者到设备

        /*      对象：覆盖定义从Object类继承的方法      */

        public override string ToString()
        {
            String result;
            result = "科室编号：" + _id + "\r\n";
            result += "科室名称：" + _name + "\r\n";
            return result;
        }

        /*      对象：构造与析构方法      */

        //构造方法（2个参数）
        public DeviceDepartment(String id, String name)
        {
            SetId(id);
            SetName(name);
            _device = new Device[4];
            _distribution = 0;
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

        public Device[] GetDevice()
        {
            return _device;
        }

        public Int32 GetDistribution()
        {
            return _distribution;
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

        //修改_distribution，使其指向本科室工作时间最少的设备
        public Boolean ChangeDistribution()
        {
            if
                (
                    _device[1] != null &&
                    _device[2] != null &&
                    _device[3] != null
                )
            {
                if (_device[1].GetPatientWait().GetSizeUsed() <= _device[2].GetPatientWait().GetSizeUsed())
                {
                    if (_device[1].GetPatientWait().GetSizeUsed() <= _device[3].GetPatientWait().GetSizeUsed())
                    {
                        _distribution = 1;
                    }
                    else
                    {
                        _distribution = 3;
                    }
                }
                else
                {
                    if (_device[2].GetPatientWait().GetSizeUsed() <= _device[3].GetPatientWait().GetSizeUsed())
                    {
                        _distribution = 2;
                    }
                    else
                    {
                        _distribution = 3;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
