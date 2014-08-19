using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimulatedClinic
{
    class SequentialQueue<T>
    {
        /*      类：字段      */

        //状态常量
        public const Int32 OkNi = 2;                //当前操作成功，但下次操作不可行
        public const Int32 Ok = 1;                  //操作成功
        public const Int32 Infeasible = 0;	        //操作不可行（实际未操作）
        public const Int32 OutOfMemory = -1;        //无法分配所需的内存

        /*      类：方法      */

        //判定两个T类对象是否相同
        public static Boolean AreEqual<TAreEqual>(TAreEqual t1, TAreEqual t2)
        {
            return EqualityComparer<TAreEqual>.Default.Equals(t1, t2);
        }

        /*      对象：字段      */

        //队列内容
        T[] _content;

        //队列“指针”
        Int32 _front;            //头“指针”
        Int32 _rear;             //尾“指针”

        //队列当前状态
        Int32 _sizeAll;          //队列最大长度
        Int32 _sizeUsed;         //队列已使用长度
        Boolean _isEmpty;        //队列是否为空
        Boolean _isFull;         //队列是否为满

        /*      对象：构造与析构方法      */

        //构造方法（1个参数）
        public SequentialQueue(Int32 s)
        {
            init(s);
        }

        /*      对象：功能方法      */

        //Get方法系列

        public Int32 GetSizeAll()
        {
            return _sizeAll;
        }

        public Int32 GetSizeUsed()
        {
            return _sizeUsed;
        }

        public Boolean GetIsEmpty()
        {
            return _isEmpty;
        }

        public Boolean GetIsFull()
        {
            return _isFull;
        }

        //循环顺序队列的初始化
        public Int32 init(Int32 i)
        {
            _sizeAll = i;
            try
            {
                _content = new T[_sizeAll];
            }
            catch (OutOfMemoryException)
            {
                return OutOfMemory;
            }
            _front = _rear = 0;
            _sizeUsed = 0;
            _isEmpty = true;
            _isFull = false;
            return Ok;
        }

        //元素elem进队
        public Int32 enterElem(T elem)
        {
            if (_sizeUsed >= _sizeAll)
            {
                return Infeasible;
            }
            _content[_rear] = elem;
            _rear = (_rear + 1) % _sizeAll;
            _sizeUsed += 1;
            _isEmpty = false;
            if (_sizeUsed == _sizeAll)
            {
                _isFull = true;
                return OkNi;
            }
            else
            {
                return Ok;
            }
        }

        //元素elem出队
        public Int32 quitElem(ref T elem)
        {
            if (_sizeUsed == 0)
            {
                return Infeasible;
            }
            elem = _content[_front];
            _front = (_front + 1) % _sizeAll;
            _sizeUsed -= 1;
            _isFull = false;
            if (_sizeUsed == 0)
            {
                _isEmpty = true;
                return OkNi;
            }
            else
            {
                return Ok;
            }
        }

        //元素elem出队（不返回出队元素）
        public Int32 quitElem()
        {
            if (_sizeUsed == 0)
            {
                return Infeasible;
            }
            _front = (_front + 1) % _sizeAll;
            _sizeUsed -= 1;
            _isFull = false;
            if (_sizeUsed == 0)
            {
                _isEmpty = true;
                return OkNi;
            }
            else
            {
                return Ok;
            }
        }

        //循环顺序队列的清空
        public void clear()
        {
            while (quitElem() != Infeasible)
            { }
        }

        //查找elem元素，返回元素位置到location
        public void ValueToIndex(ref Int32 location, T elem)
        {
            Int32 p;
            location = 0;
            Int32 counter;
            for (p = _front, counter = 1; counter <= _sizeUsed; counter++)
            {
                if (AreEqual<T>(_content[p], elem))
                {
                    location = counter;
                    break;
                }
                p = (p + 1) % _sizeAll;
            }
        }

        //由元素在队列中的位置取出该元素
        public T IndexToValue(Int32 index)
        {
            if (_isEmpty)
            {
                return default(T);
            }
            Int32 pointer = _front;
            if (index <= _sizeUsed)
            {
                for (Int32 i = 1; i <= _sizeUsed; i++)
                {
                    if (i == index)
                    {
                        return _content[pointer];
                    }
                    pointer = (pointer + 1) % _sizeAll;
                }
                return default(T);
            }
            else
            {
                return default(T);
            }
        }
    }
}
