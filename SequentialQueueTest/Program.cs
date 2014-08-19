using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimulatedClinic
{
    class Program
    {
        static void Main(string[] args)
        {
            String temp = "";

            //初始化测试
            SequentialQueue<String> test1 = new SequentialQueue<String>(10);
            SequentialQueue<String> test2 = new SequentialQueue<String>(6);
            //输出
            display(test1, "test1");
            display(test2, "test2");
            Console.ReadLine();
            Console.Clear();

            //入队测试1
            for (int i = 1; i <= 8; i++)
            {
                test1.enterElem(i.ToString());
                test2.enterElem(i.ToString());
                //输出
                display(test1, "test1");
                display(test2, "test2");
                Console.ReadLine();
                Console.Clear();
            }

            //出队测试
            for (int i = 1; i <= 8; i++)
            {
                test1.quitElem();
                test2.quitElem(ref temp);
                //输出
                display(test1, "test1");
                display(test2, "test2");
                Console.ReadLine();
                Console.Clear();
            }

            //入队测试2
            for (int i = 1; i <= 16; i++)
            {
                test1.enterElem(i.ToString());
                test2.enterElem(i.ToString());
            }
            //输出
            display(test1, "test1");
            display(test2, "test2");
            Console.ReadLine();
            Console.Clear();

            //搜索测试
            Int32 location = 0;
            display(test1, "test1");
            for (Int32 i = 1; i <= 12; i++)
            {
                Console.Out.WriteLine("查找数据：" + i);
                test1.ValueToIndex(ref location, i.ToString());
                Console.Out.WriteLine("位置信息：" + location);
                Console.Out.WriteLine();
            }
            Console.ReadLine();
            Console.Clear();
            display(test2, "test2");
            for (Int32 i = 1; i <= 12; i++)
            {
                Console.Out.WriteLine("查找数据：" + i);
                test2.ValueToIndex(ref location, i.ToString());
                Console.Out.WriteLine("位置信息：" + location);
                Console.Out.WriteLine();
            }
            Console.ReadLine();
            Console.Clear();

            //清空测试
            test1.clear();
            test2.clear();
            //输出
            display(test1, "test1");
            display(test2, "test2");
            Console.ReadLine();
            Console.Clear();

            //GetValue方法在已经被多次用在display方法中，无需再次测试

            //结束
            Console.Out.WriteLine("测试结束！");
            Console.Out.WriteLine();
            Console.Out.WriteLine("按任意键退出...");
            Console.ReadLine();
        }

        //循环顺序队列sq的结构和功能完全正确 当且仅当 循环顺序队列sq在方法中输出正确的结果
        static void display(SequentialQueue<String> sq, String s)
        {
            //Node pr;
            Console.Out.WriteLine("循环顺序队列:");
            Console.Out.WriteLine(s);
            //当前链表的状态
            Console.Out.WriteLine("当前循环顺序队列的状态：");
            Console.Out.Write("sizeAll = " + sq.GetSizeAll() + "   ");
            Console.Out.Write("sizeUsed = " + sq.GetSizeUsed() + "   ");
            Console.Out.Write("isEmpty = " + sq.GetIsEmpty() + "   ");
            Console.Out.WriteLine("isFull = " + sq.GetIsFull() + "   ");
            //当前的链表内容为
            Console.Out.WriteLine("当前的循环顺序队列内容为：");
            if (sq.GetIsEmpty() ==false)
            {
                String value = "";
                for (Int32 i = 1; i <= sq.GetSizeUsed(); i++)
                {
                    value = sq.IndexToValue(i);
                    Console.Out.Write(value + "   ");
                }
                Console.Out.WriteLine("");
            }
            else
            {
                Console.Out.WriteLine("");
            }
            Console.Out.WriteLine();
        }
    }
}
