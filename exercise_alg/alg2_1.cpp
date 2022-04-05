#include <iostream>
#include <vector>
using namespace std;

int Select(vector<int> &a, int front, int l)
{
    
    int k = front;  
    for (int i = front + 1; i < l; i++)
    {
        if (a[i] < a[k])
            k = i;
    }
    if (k != front)
    {
        int t = a[front];
        a[front] = a[k];
        a[k] = t;
    }

    return a[front];
}

int main()
{
    int n = 0;
    cout << "待排序的元素的个数为：" << endl;
    cin >> n;
    vector<int>a;
    
    cout << "依次输入待排序的元素：" << endl;
    int m;
    for (int i = 0; i < n; i++)
    {
        cin >> m;
        a.push_back(m);
    }

    cout << "从小到大排列的结果为：";
    for (int j = 0; j < n; j++)
    {
        cout << Select(a, j, n) << "  ";
    }
}
