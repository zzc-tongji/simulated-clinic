// SequentialQueue.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"
#include <iostream>
#include <iomanip>
#include <cmath>
#include <stdlib.h>
using namespace std;

/* ����'STATUS'����ȡֵ */

typedef int STATUS;
const int OK = 1;
const int ERROR = 0;
const int INFEASIBLE = -1;  //������

//----------------------------------------------------------------------------------------------------//

/*                                            ѭ��˳�����                                            */

/* ��������Ԫ�ء����ݽṹ������ȫ�ֱ��� */

typedef int SQ_QUEUE_ELEM_TYPE;

struct SQ_QUEUE
{
    SQ_QUEUE_ELEM_TYPE *base_q;
    SQ_QUEUE_ELEM_TYPE *front;
    SQ_QUEUE_ELEM_TYPE *rear;
    int size_used;
    int size_all;
};

/* �������ݲ��� */

//��������

//ѭ��˳����еĳ�ʼ��
STATUS SQ_QUEUE_init(SQ_QUEUE &queue, int def);
//ѭ��˳����е�����
STATUS SQ_QUEUE_delete(SQ_QUEUE &queue);
//ѭ��˳����е�����
STATUS SQ_QUEUE_input(SQ_QUEUE &queue);
//ѭ��˳����е����
STATUS SQ_QUEUE_output(const SQ_QUEUE &queue);
//Ԫ��elem����
STATUS SQ_QUEUE_enter(SQ_QUEUE &queue, const SQ_QUEUE_ELEM_TYPE &elem);
//Ԫ��elem����
STATUS SQ_QUEUE_quit(SQ_QUEUE &queue, SQ_QUEUE_ELEM_TYPE &elem);
//����elemԪ�أ�����Ԫ��λ�õ�location
STATUS SQ_QUEUE_search_ELEM(const SQ_QUEUE &queue, int &location, const SQ_QUEUE_ELEM_TYPE &elem);
//ѭ��˳����е����
STATUS SQ_QUEUE_clear(SQ_QUEUE &queue);

//���ߺ���

//����Ԫ��
void SQ_QUEUE_TOOL_input(SQ_QUEUE_ELEM_TYPE &a);
//�������
//void SQ_QUEUE_TOOL_input(int &a);

/* �������ݲ��� */

//ѭ��˳����еĳ�ʼ��
STATUS SQ_QUEUE_init(SQ_QUEUE &queue, int def)
{
    int init_size;
    if (def == 0)
    {
        cout << "��Ҫ����ѭ��˳����г�ʼ��" << endl;
        cout << endl;
        cout << "�����ѭ��˳����е����Ԫ��������" << endl;
        cout << "����������Χ1~~1248576�����´γ�ʼ��ǰ�����޸ģ�" << endl;
        SQ_QUEUE_TOOL_input(init_size);
        while (init_size<1 || init_size>1248576)
        {
            cout << "���ݲ��Ϸ������������룺";
            SQ_QUEUE_TOOL_input(init_size);
        }
        cout << "ѭ��˳���������������" << init_size << "�����ݡ�" << endl;
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

//ѭ��˳����е�����
STATUS SQ_QUEUE_delete(SQ_QUEUE &queue)
{
    if (!queue.base_q) free(queue.base_q);
    queue.front = queue.rear = NULL;
    queue.size_used = 0;
    queue.size_all = 0;
    return OK;
}

//ѭ��˳����е�����
STATUS SQ_QUEUE_input(SQ_QUEUE &queue)
{
    int input_size = 0;
    int counter = 0;
    SQ_QUEUE_ELEM_TYPE temp;
    SQ_QUEUE_clear(queue);
    cout << "�������Ҫ�����Ԫ����������������Χ0~~" << queue.size_all << "��:";
    SQ_QUEUE_TOOL_input(input_size);
    while (input_size<0 || input_size>queue.size_all)
    {
        cout << "���ݲ��Ϸ������������룺";
        SQ_QUEUE_TOOL_input(input_size);
    }
    cout << "��Ҫ����" << input_size << "�����ݡ�" << endl;
    cout << endl;
    if (!input_size)
    {
        return OK;
    }
    cout << "�������������Ԫ�أ�ÿ��Ԫ��֮���ûس��ָ���" << endl;
    cout << setiosflags(ios::fixed) << setprecision(0) << "����������Χ" << (-pow(2.0, (double)(8 * sizeof(int)-1))) << "~~" << (pow(2.0, (double)(8 * sizeof(int)-1)) - 1) << "��" << endl;
    for (counter = 0; counter<input_size; counter++)
    {
        cout << "No." << setw(7) << setfill('0') << counter + 1 << " ";
        SQ_QUEUE_TOOL_input(temp);
        SQ_QUEUE_enter(queue, temp);
    }
    return OK;
}

//ѭ��˳����е����
STATUS SQ_QUEUE_output(const SQ_QUEUE &queue)
{
    int counter;
    SQ_QUEUE_ELEM_TYPE *p;
    p = queue.front;
    cout << "*************************************" << endl;
    cout << "* ѭ��˳������ܴ�С��" << setw(7) << setfill(' ') << queue.size_all << "��Ԫ�� *" << endl;
    cout << "* ѭ��˳�������ʹ�ã�" << setw(7) << setfill(' ') << queue.size_used << "��Ԫ�� *" << endl;
    cout << "* ѭ��˳��������ݣ�                *" << endl;
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
    cout << "* ע����ԪΪ��ͷ��ĩԪΪ��β��      *" << endl;
    cout << "*************************************" << endl;
    cout << endl;
    return OK;
}

//Ԫ��elem����
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

//Ԫ��elem����
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

//����elemԪ�أ�����Ԫ��λ�õ�location
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

//ѭ��˳����е����
STATUS SQ_QUEUE_clear(SQ_QUEUE &queue)
{
    queue.front = queue.base_q;
    queue.rear = queue.base_q;
    queue.size_used = 0;
    return OK;
}

//����Ԫ��
void SQ_QUEUE_TOOL_input(SQ_QUEUE_ELEM_TYPE &a)
{
    if (!(cin >> a))
    {
        cout << endl;
        cout << "****�����쳣��****" << endl;
        cout << endl;
        cout << "�����˳�..." << endl;
        cout << endl;
        system("PAUSE");
        exit(INFEASIBLE);
    }
}

/*
//�������
void SQ_QUEUE_TOOL_input(int &a)
{
if (! (cin >> a))
{
cout << endl;
cout << "****�����쳣��****" << endl;
cout<< endl;
cout << "�����˳�..." << endl;
cout << endl;
system("PAUSE");
exit(INFEASIBLE);
}
}
*/

//----------------------------------------------------------------------------------------------------//

/* ������ʵ�� */

//����Ԫ��
void input(SQ_QUEUE_ELEM_TYPE &a)
{
    if (!(cin >> a))
    {
        cout << endl;
        cout << "****�����쳣��****" << endl;
        cout << endl;
        cout << "�����˳�..." << endl;
        cout << endl;
        system("PAUSE");
        exit(INFEASIBLE);
    }
}

/*
//�������
void input(int &a)
{
if (! (cin >> a))
{
cout << endl;
cout << "****�����쳣��****" << endl;
cout<< endl;
cout << "�����˳�..." << endl;
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
            cout << "****�����ʼ���������ɹ���****" << endl;
            cout << endl;
            system("PAUSE");
        }
    }
    else
    {
        cout << "****�����ʼ��ʧ��...****" << endl;
        cout << endl;
        cout << "�����˳�..." << endl;
        cout << endl;
        system("PAUSE");
        return INFEASIBLE;
    }
    while (1)
    {
        cout << endl;
        cout << "**********************************************************************" << endl;
        cout << "*                                                                    *" << endl;
        cout << "*                            ѭ��˳�����                            *" << endl;
        cout << "*                                                                    *" << endl;
        cout << "*  ����:                                                             *" << endl;
        cout << "*      00.��ʼ������                                                 *" << endl;
        cout << "*      99.�˳�����                                                   *" << endl;
        cout << "*                                                                    *" << endl;
        cout << "*  ����:                                                             *" << endl;
        cout << "*      01.ѭ��˳�����l������                                        *" << endl;
        cout << "*      02.ѭ��˳�����l�����                                        *" << endl;
        cout << "*      03.Ԫ��e����                                                  *" << endl;
        cout << "*      04.Ԫ��e����                                                  *" << endl;
        cout << "*      05.����ѭ��˳�����l��Ԫ��e������Ԫ��e��ѭ��˳������е�λ��  *" << endl;
        cout << "*      06.ѭ��˳�����l�����                                        *" << endl;
        cout << "*                                                                    *" << endl;
        cout << "*  ע�⣺                                                            *" << endl;
        cout << "*      �밴Ҫ�����룡���Ϸ������뽫���ܵ��³���ֱ���˳���            *" << endl;
        cout << "*                                                                    *" << endl;
        cout << "**********************************************************************" << endl;
        cout << endl;
        cout << "��ѡ��";
        input(func);
        cout << endl;
        switch (func)
        {
        case 0: //��ʼ������
        {
                    if (SQ_QUEUE_delete(l) && SQ_QUEUE_init(l, 0))
                    {
                        cout << "****��ʼ���������ɹ���****" << endl;
                        cout << endl;
                    }
                    else
                    {
                        cout << "****��ʼ��������ʧ�ܣ�****" << endl;
                        cout << endl;
                        cout << "�����˳�..." << endl;
                        cout << endl;
                        system("PAUSE");
                        return INFEASIBLE;
                    }
                    system("PAUSE");
                    break;
        }
        case 99: //�˳�����
        {
                     cout << "�����˳�..." << endl;
                     cout << endl;
                     system("PAUSE");
                     return 0;
        }
        case 1: //ѭ��˳�����l������
        {
                    cout << "��Ҫ����ѭ��˳�����l" << endl;
                    cout << endl;
                    if (SQ_QUEUE_input(l))
                    {
                        cout << "****�����ɹ���****" << endl;
                        cout << endl;
                        SQ_QUEUE_output(l);
                    }
                    else
                    {
                        cout << "****����ʧ�ܣ�****" << endl;
                        cout << endl;
                    }
                    system("PAUSE");
                    break;
        }
        case 2: //ѭ��˳�����l�����
        {
                    cout << "****ѭ��˳�����l������£�****" << endl;
                    cout << endl;
                    SQ_QUEUE_output(l);
                    system("PAUSE");
                    break;
        }
        case 3: //Ԫ��e����
        {
                    cout << "ѭ��˳�����l��" << endl;
                    cout << endl;
                    SQ_QUEUE_output(l);
                    cout << "���������ӵ�Ԫ�أ�";
                    cout << setiosflags(ios::fixed) << "����������Χ" << (int)(-pow(2.0, (double)(8 * sizeof(int)-1))) << "~~" << (int)(pow(2.0, (double)(8 * sizeof(int)-1))) << "��" << endl;
                    input(e);
                    cout << "��Ҫ��Ԫ��" << e << "���ӡ�" << endl;
                    cout << endl;
                    if (SQ_QUEUE_enter(l, e))
                    {
                        cout << "****�����ɹ���****" << endl;
                        cout << endl;
                        SQ_QUEUE_output(l);
                    }
                    else
                    {
                        cout << "ѭ��˳�����������" << endl;
                        cout << endl;
                        cout << "****����ʧ�ܣ�****" << endl;
                        cout << endl;
                    }
                    system("PAUSE");
                    break;
        }
        case 4: //Ԫ��e����
        {
                    cout << "ѭ��˳�����l��" << endl;
                    cout << endl;
                    SQ_QUEUE_output(l);
                    cout << "��Ҫ�ö�βԪ�س��ӡ�" << endl;
                    cout << endl;
                    if (SQ_QUEUE_quit(l, e))
                    {
                        cout << "����Ԫ�أ�" << e << "��" << endl;
                        cout << endl;
                        cout << "****�����ɹ���****" << endl;
                        cout << endl;
                        SQ_QUEUE_output(l);
                    }
                    else
                    {
                        cout << "ѭ��˳�����Ϊ�ա�" << endl;
                        cout << endl;
                        cout << "****����ʧ�ܣ�****" << endl;
                        cout << endl;
                    }
                    system("PAUSE");
                    break;
        }
        case 5: //����ѭ��˳�����l��Ԫ��e������Ԫ��e��ѭ��˳������е�λ��
        {
                    SQ_QUEUE_output(l);
                    cout << setiosflags(ios::fixed) << "�������Ҫ���ҵ�Ԫ�أ���������Χ" << (int)(-pow(2.0, (double)(8 * sizeof(int)-1))) << "~~" << (int)(pow(2.0, (double)(8 * sizeof(int)-1))) << "��:";
                    input(e);
                    cout << endl;
                    cout << "��Ҫ���ҵ�Ԫ��Ϊ" << e << "��" << endl;
                    cout << endl;
                    SQ_QUEUE_search_ELEM(l, loc, e);
                    if (!loc)
                    {
                        cout << "Ԫ��" << e << "��������ѭ��˳�����l�С�" << endl;
                        cout << endl;
                    }
                    else
                    {
                        cout << "Ԫ��" << e << "��ѭ��˳�����l�е����ΪNO." << setw(7) << setfill('0') << loc << "��" << endl;
                        cout << endl;
                    }
                    system("PAUSE");
                    break;
        }
        case 6: //ѭ��˳�����l�����
        {
                    SQ_QUEUE_clear(l);
                    cout << "��Ҫ���ѭ��˳�����l" << endl;
                    cout << endl;
                    cout << "****�����ɹ���****" << endl;
                    cout << endl;
                    SQ_QUEUE_output(l);
                    system("PAUSE");
                    break;
        }
        default:
        {
                   cout << endl;
                   cout << "****����������������룡****" << endl;
                   cout << endl;
                   system("PAUSE");
                   break;
        }
        }
    }
}
