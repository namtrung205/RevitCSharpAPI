# Tim uoc chung lon nhat 2 so num1, num2

num_1 = 24
num_2 = 36

def ucln(num1, num2):
	if num1 == 0 or num2 == 0:
		return num1+num2
	while (num1 != num2):
		if(num1 > num2):
			num1 -= num2
		else:
			num2 -= num1
	return num1


def bnnn(num1, num2):
	return num1*num2/ucln(num1, num2)


print(bnnn(num_1, num_2))