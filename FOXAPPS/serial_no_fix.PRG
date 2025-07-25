OPEN DATABASE W:\foxapps\shortdata
USE serialno

N = 0

SCAN 
	IF ATC("*", serial_no) > 0
		REPLACE serial_no WITH CHRTRAN(serial_no, '*', '')
		N = N + 1
	ENDIF
ENDSCAN

MESSAGEBOX(STR(N) + " Serial No's Fixed.")

