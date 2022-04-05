#include <iostream>
using namespace std;

double alg(double x, int n) {
	if (n == 1)
		return x;  //递归的出口

	if (n % 2 == 0)
		return alg(x, n / 2) * alg(x, n / 2);  //在指数为偶数的情况下，x^n=x^(n/2)*a^(n/2)
	else
		return x * alg(x, n / 2) * alg(x, n / 2);  //在指数为奇数的情况下，x^n=x*x^(n/2)*x^(n/2)
}

int main()
{
	double x;
	int n;
	cout << "输入的x值为：" << endl;
	cin >> x;
	cout << "输入的n值为：" << endl;
	cin >> n;

	if (n == 0)
		cout << "x^n的值为：1" << endl;
	else if (n > 0)
		cout << "x^n的值为：" << alg(x, n) << endl;
	else
		cout << "x^n的值为：" << 1 / alg(x, -n) << endl;  //当指数为负数时，转化为正数调用求幂函数

	return 0;
}
