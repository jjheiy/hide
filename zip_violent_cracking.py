import zipfile

def readPasswords(filename):
	with open(filename, 'rb') as f:
		passwords = f.readlines()
	return passwords
	
def testPasswords(zip_file, passwords):
	for password in passwords:
		print('Test{}'.format(password[:-1].decode()))
		try:
			zip_file.extractall(pwd=password[:-1])
		except RuntimeError:
			continue
		return password
def main(zip_filename, passwords_filename):
	zip_file = zipfile.ZipFile(zip_filename)
	passwords = readPasswords(passwords_filename)
	password = testPasswords(zip_file, passwords)
	if password is not None:
		print('Password is {}'.format(password[:-1].decode()))
	else:
		print('No find password in {}'.format(passwords_filename))
				
if __name__ == '__main__':
	import sys
	if len(sys.argv) == 3:
		main(sys.argv[1], sys.argv[2])
	else:
		print('Please set zip filepath and passwords list filepath')
	