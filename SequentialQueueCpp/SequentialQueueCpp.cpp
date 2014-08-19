// SequentialQueue.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include <iostream>
#include <iomanip>
#include <cmath>
#include <stdlib.h>
using namespace std;

/* 定义'STATUS'及其取值 */

typedef int STATUS;
const int OK = 1;
const int ERROR = 0;
const int INFEASIBLE = -1;  //不可行

//----------------------------------------------------------------------------------------------------//

/*                                            循环顺序队列                                            */

/* 定义数据元素、数据结构和其他全局变量 */

typedef int SQ_QUEUE_ELEM_TYPE;

struct SQ_QUEUE
{
    SQ_QUEUE_ELEM_TYPE *base_q;
    SQ_QUEUE_ELEM_TYPE *front;
    SQ_QUEUE_ELEM_TYPE *rear;
    int size_used;
    int size_all;
};

/* 声明数据操作 */

//基本功能

//循环顺序队列的初始化
STATUS SQ_QUEUE_init(SQ_QUEUE &queue, int def);
//循环顺序队列的销毁
STATUS SQ_QUEUE_delete(SQ_QUEUE &queue);
//循环顺序队列的输入
STATUS SQ_QUEUE_input(SQ_QUEUE &queue);
//循环顺序队列的输出
STATUS SQ_QUEUE_output(const SQ_QUEUE &queue);
//元素elem进队
STATUS SQ_QUEUE_enter(SQ_QUEUE &queue, const SQ_QUEUE_ELEM_TYPE &elem);
//元素elem出队
STATUS SQ_QUEUE_quit(SQ_QUEUE &queue, SQ_QUEUE_ELEM_TYPE &elem);
//查找elem元素，返回元素位置到location
STATUS SQ_QUEUE_search_ELEM(const SQ_QUEUE &queue, int &location, const SQ_QUEUE_ELEM_TYPE &elem);
//循环顺序队列的清空
STATUS SQ_QUEUE_clear(SQ_QUEUE &queue);

//工具函数

//输入元素
void SQ_QUEUE_TOOL_input(SQ_QUEUE_ELEM_TYPE &a);
//输入序号
//void SQ_QUEUE_TOOL_input(int &a);

/* 定义数据操作 */

//循环顺序队列的初始化
STATUS SQ_QUEUE_init(SQ_QUEUE &queue, int def)
{
    int init_size;
    if (def == 0)
    {
        cout << "需要进行循环顺序队列初始化" << endl;
        cout << endl;
        cout << "请给出循环顺序队列的最大元素数量：" << endl;
        cout << "（整数，范围1~~1248576，在下次初始化前不可修改）" << endl;
        SQ_QUEUE_TOOL_input(init_size);
        while (init_size<1 || init_size>1248576)
        {
            cout << "数据不合法，请重新输入：";
            SQ_QUEUE_TOOL_input(init_size);
        }
        cout << "循环顺序队列最多可以容纳" << init_size << "个数据。" << endl;
        cout << endl;
    }
    else
    {
        init_size = def;
    }
    queue.base_q = (SQ_QUEUE_ELEM_TYPE *)calloc(sizeof(SQ_QUEUE_ELEM_TYPE), init_size);
    if (!queue.base_q) return ERROR;
    queue.front = queue.rear = queue.base_q;
    queue.size_used = 0;
    queue.size_all = init_size;
    return OK;
}

//循环顺序队列的销毁
STATUS SQ_QUEUE_delete(SQ_QUEUE &queue)
{
    if (!queue.base_q) free(queue.base_q);
    queue.front = queue.rear = NULL;
    queue.size_used = 0;
    queue.size_all = 0;
    return OK;
}

//循环顺序队列的输入
STATUS SQ_QUEUE_input(SQ_QUEUE &queue)
{
    int input_size = 0;
    int counter = 0;
    SQ_QUEUE_ELEM_TYPE temp;
    SQ_QUEUE_clear(queue);
    cout << "请给出需要输入的元素数量（整数，范围0~~" << queue.size_all << "）:";
    SQ_QUEUE_TOOL_input(input_size);
    while (input_size<0 || input_size>queue.size_all)
    {
        cout << "数据不合法，请重新输入：";
        SQ_QUEUE_TOOL_input(input_size);
    }
    cout << "需要输入" << input_size << "个数据。" << endl;
    cout << endl;
    if (!input_size)
    {
        return OK;
    }
    cout << "请依次输入各个元素，每个元素之间用回车分隔：" << endl;
    cout << setiosflags(ios::fixed) << setprecision(0) << "（整数，范围" << (-pow(2.0, (double)(8 * sizeof(int)-1))) << "~~" << (pow(2.0, (double)(8 * sizeof(int)-1)) - 1) << "）" << endl;
    for (counter = 0; counter<input_size; counter++)
    {
        cout << "No." << setw(7) << setfill('0') << counter + 1 << " ";
        SQ_QUEUE_TOOL_input(temp);
        SQ_QUEUE_enter(queue, temp);
    }
    return OK;
}

//循环顺序队列的输出
STATUS SQ_QUEUE_output(const SQ_QUEUE &queue)
{
    int counter;
    SQ_QUEUE_ELEM_TYPE *p;
    p = queue.front;
    cout << "*************************************" << endl;
    cout << "* 循环顺序队列总大小：" << setw(7) << setfill(' ') << queue.size_all << "个元素 *" << endl;
    cout << "* 循环顺序队列已使用：" << setw(7) << setfill(' ') << queue.size_used << "个元素 *" << endl;
    cout << "* 循环顺序队列内容：                *" << endl;
    for (counter = 0; counter<queue.size_used; counter++)
    {
        cout << "*    No." << setw(7) << setfill('0') << counter + 1 << " ";
        cout << setw(17) << setfill(' ') << *p << "   *" << endl;
        if (p - queue.base_q >= queue.size_all - 1)
        {
            p = queue.base_q;
        }
        else
        {
            p++;
        }
    }
    cout << "*                                   *" << endl;
    cout << "* 注：首元为队头，末元为队尾。      *" << endl;
    cout << "*************************************" << endl;
    cout << endl;
    return OK;
}

//元素elem进队
STATUS SQ_QUEUE_enter(SQ_QUEUE &queue, const SQ_QUEUE_ELEM_TYPE &elem)
{
    if (queue.size_used >= queue.size_all)
    {
        return ERROR;
    }
    *queue.rear = elem;
    if (queue.rear - queue.base_q >= queue.size_all - 1)
    {
        queue.rear = queue.base_q;
    }
    else
    {
        queue.rear += 1;
    }
    queue.size_used++;
    return OK;
}

//元素elem出队
STATUS SQ_QUEUE_quit(SQ_QUEUE &queue, SQ_QUEUE_ELEM_TYPE &elem)
{
    if (queue.size_used == 0)
    {
        return ERROR;
    }
    elem = *queue.front;
    if (queue.front - queue.base_q >= queue.size_all - 1)
    {
        queue.front = queue.base_q;
    }
    else
    {
        queue.front += 1;
    }
    queue.size_used--;
    return OK;
}

//查找elem元素，返回元素位置到location
STATUS SQ_QUEUE_search_ELEM(const SQ_QUEUE &queue, int &location, const SQ_QUEUE_ELEM_TYPE &elem)
{
    SQ_QUEUE_ELEM_TYPE *p;
    location = 0;
    int counter;
    for (p = queue.front, counter = 1; counter <= queue.size_used; counter++)
    {
        if (*p == elem)
        {
            location = counter;
            break;
        }
        if (p - queue.base_q >= queue.size_all - 1)
        {
            p = queue.base_q;
        }
        else
        {
            p++;
        }

    }
    return OK;
}

//循环顺序队列的清空
STATUS SQ_QUEUE_clear(SQ_QUEUE &queue)
{
    queue.front = queue.base_q;
    queue.rear = queue.base_q;
    queue.size_used = 0;
    return OK;
}

//输入元素
void SQ_QUEUE_TOOL_input(SQ_QUEUE_ELEM_TYPE &a)
{
    if (!(cin >> a))
    {
        cout << endl;
        cout << "****输入异常！****" << endl;
        cout << endl;
        cout << "程序退出..." << endl;
        cout << endl;
        system("PAUSE");
        exit(INFEASIBLE);
    }
}

/*
//输入序号
void SQ_QUEUE_TOOL_input(int &a)
{
if (! (cin >> a))
{
cout << endl;
cout << "****输入异常！****" << endl;
cout<< endl;
cout << "程序退出..." << endl;
cout << endl;
system("PAUSE");
exit(INFEASIBLE);
}
}
*/

//----------------------------------------------------------------------------------------------------//

/* 主函数实现 */

//输入元素
void input(SQ_QUEUE_ELEM_TYPE &a)
{
    if (!(cin >> a))
    {
        cout << endl;
        cout << "****输入异常！****" << endl;
        cout << endl;
        cout << "程序退出..." << endl;
        cout << endl;
        system("PAUSE");
        exit(INFEASIBLE);
    }
}

/*
//输入序号
void input(int &a)
{
if (! (cin >> a))
{
cout << endl;
cout << "****输入异常！****" << endl;
cout<< endl;
cout << "程序退出..." << endl;
cout << endl;
system("PAUSE");
exit(INFEASIBLE);
}
}
*/

int main()
{
    SQ_QUEUE l;
    SQ_QUEUE_ELEM_TYPE e;
    int loc;
    int func, init;
    init = 0;
    if (SQ_QUEUE_init(l, init))
    {
        if (init == 0)
        {
            cout << "****程序初始化，操作成功！****" << endl;
            cout << endl;
            system("PAUSE");
        }
    }
    else
    {
        cout << "****程序初始化失败...****" << endl;
        cout << endl;
        cout << "程序退出..." << endl;
        cout << endl;
        system("PAUSE");
        return INFEASIBLE;
    }
    while (1)
    {
        cout << endl;
        cout << "**********************************************************************" << endl;
        cout << "*                                                                    *" << endl;
        cout << "*                            循环顺序队列                            *" << endl;
        cout << "*                                                                    *" << endl;
        cout << "*  控制:                                                             *" << endl;
        cout << "*      00.初始化程序                                                 *" << endl;
        cout << "*      99.退出程序                                                   *" << endl;
        cout << "*                                                                    *" << endl;
        cout << "*  功能:                                                             *" << endl;
        cout << "*      01.循环顺序队列l的输入                                        *" << endl;
        cout << "*      02.循环顺序队列l的输出                                        *" << endl;
        cout << "*      03.元素e进队                                                  *" << endl;
        cout << "*      04.元素e出队                                                  *" << endl;
        cout << "*      05.查找循环顺序队列l的元素e，返回元素e在循环顺序队列中的位置  *" << endl;
        cout << "*      06.循环顺序队列l的清空                                        *" << endl;
        cout << "*                                                                    *" << endl;
        cout << "*  注意：                                                            *" << endl;
        cout << "*      请按要求输入！不合法的输入将可能导致程序直接退出！            *" << endl;
        cout << "*                                                                    *" << endl;
        cout << "**********************************************************************" << endl;
        cout << endl;
        cout << "请选择：";
        input(func);
        cout << endl;
        switch (func)
        {
        case 0: //初始化程序
        {
                    if (SQ_QUEUE_delete(l) && SQ_QUEUE_init(l, 0))
                    {
                        cout << "****初始化，操作成功！****" << endl;
                        cout << endl;
                    }
                    else
                    {
                        cout << "****初始化，操作失败！****" << endl;
                        cout << endl;
                        cout << "程序退出..." << endl;
                        cout << endl;
                        system("PAUSE");
                        return INFEASIBLE;
                    }
                    system("PAUSE");
                    break;
        }
        case 99: //退出程序
        {
                     cout << "程序退出..." << endl;
                     cout << endl;
                     system("PAUSE");
                     return 0;
        }
        case 1: //循环顺序队列l的输入
        {
                    cout << "需要输入循环顺序队列l" << endl;
                    cout << endl;
                    if (SQ_QUEUE_input(l))
                    {
                        cout << "****操作成功！****" << endl;
                        cout << endl;
                        SQ_QUEUE_output(l);
                    }
                    else
                    {
                        cout << "****操作失败！****" << endl;
                        cout << endl;
                    }
                    system("PAUSE");
                    break;
        }
        case 2: //循环顺序队列l的输出
        {
                    cout << "****循环顺序队列l输出如下：****" << endl;
                    cout << endl;
                    SQ_QUEUE_output(l);
                    system("PAUSE");
                    break;
        }
        case 3: //元素e进队
        {
                    cout << "循环顺序队列l：" << endl;
                    cout << endl;
                    SQ_QUEUE_output(l);
                    cout << "请给出需进队的元素：";
                    cout << setiosflags(ios::fixed) << "（整数，范围" << (int)(-pow(2.0, (double)(8 * sizeof(int)-1))) << "~~" << (int)(pow(2.0, (double)(8 * sizeof(int)-1))) << "）" << endl;
                    input(e);
                    cout << "需要让元素" << e << "进队。" << endl;
                    cout << endl;
                    if (SQ_QUEUE_enter(l, e))
                    {
                        cout << "****操作成功！****" << endl;
                        cout << endl;
                        SQ_QUEUE_output(l);
                    }
                    else
                    {
                        cout << "循环顺序队列已满。" << endl;
                        cout << endl;
                        cout << "****操作失败！****" << endl;
                        cout << endl;
                    }
                    system("PAUSE");
                    break;
        }
        case 4: //元素e出队
        {
                    cout << "循环顺序队列l：" << endl;
                    cout << endl;
                    SQ_QUEUE_output(l);
                    cout << "需要让队尾元素出队。" << endl;
                    cout << endl;
                    if (SQ_QUEUE_quit(l, e))
                    {
                        cout << "出队元素：" << e << "。" << endl;
                        cout << endl;
                        cout << "****操作成功！****" << endl;
                        cout << endl;
                        SQ_QUEUE_output(l);
                    }
                    else
                    {
                        cout << "循环顺序队列为空。" << endl;
                        cout << endl;
                        cout << "****操作失败！****" << endl;
                        cout << endl;
                    }
                    system("PAUSE");
                    break;
        }
        case 5: //查找循环顺序队列l的元素e，返回元素e在循环顺序队列中的位置
        {
                    SQ_QUEUE_output(l);
                    cout << setiosflags(ios::fixed) << "请给出需要查找的元素（整数，范围" << (int)(-pow(2.0, (double)(8 * sizeof(int)-1))) << "~~" << (int)(pow(2.0, (double)(8 * sizeof(int)-1))) << "）:";
                    input(e);
                    cout << endl;
                    cout << "需要查找的元素为" << e << "。" << endl;
                    cout << endl;
                    SQ_QUEUE_search_ELEM(l, loc, e);
                    if (!loc)
                    {
                        cout << "元素" << e << "不存在于循环顺序队列l中。" << endl;
                        cout << endl;
                    }
                    else
                    {
                        cout << "元素" << e << "在循环顺序队列l中的序号为NO." << setw(7) << setfill('0') << loc << "。" << endl;
                        cout << endl;
                    }
                    system("PAUSE");
                    break;
        }
        case 6: //循环顺序队列l的清空
        {
                    SQ_QUEUE_clear(l);
                    cout << "需要清空循环顺序队列l" << endl;
                    cout << endl;
                    cout << "****操作成功！****" << endl;
                    cout << endl;
                    SQ_QUEUE_output(l);
                    system("PAUSE");
                    break;
        }
        default:
        {
                   cout << endl;
                   cout << "****输入错误，请重新输入！****" << endl;
                   cout << endl;
                   system("PAUSE");
                   break;
        }
        }
    }
}
