#include<vector>
#include<iostream>
using namespace std;

vector<int> fin;

void Alg(vector<int>& a, int &times) {
    int s = a.size();
    int i = 0;
    int j = s - 1;
    //相当于前后两个指针
    while (i < j) {
        if (a[i] > a[j]) {
            a[j - 1] += a[j];
            a[j] = -1;
            j--;
            times++;
        }
        else if (a[i] < a[j]) {
            a[i + 1] += a[i];
            a[i] = -1;
            i++;
            times++;
        }
        else {
            i++;
            j--;
        }
    }
    for (int i = 0; i < s; i++)
    {
        if (a[i] > 0)
            fin.push_back(a[i]);
    }
}

int main() {
    int n;
    int times = 0;
    cout << "该序列的个数n为：";
    cin >> n;
    cout << endl;
    vector<int> z(n);//创建动态数组保存初始序列
    cout << "依次输入该序列为：";
    for (int i = 0; i < n; i++)
    {
        cin >> z[i];
    }
    Alg(z, times);
    cout << "选择次数为:" << times << endl;
    cout << "最后序列的状态为:";
    for (int i = 0; i < fin.size(); i++)
    {
        cout << fin[i] << '\t';
    }
    cout << endl;
}
