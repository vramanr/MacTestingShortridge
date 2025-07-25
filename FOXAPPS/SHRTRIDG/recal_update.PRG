OPEN DATABASE w:\foxapps\shortdata

USE precheck
SCAN
	DO CASE
		CASE !EMPTY(fh_flo_tst)
			finishdate = fh_flo_tst
		CASE !EMPTY(hdmrecalfn)	
			finishdate = hdmrecalfn
		CASE !EMPTY(admrecalfn)	
			finishdate = admrecalfn		
		OTHERWISE		
			finishdate = CTOD("")
	ENDCASE

	REPLACE rcl_finish WITH finishdate
ENDSCAN