OPEN DATABASE w:\foxapps\shortdata

USE precheck
SCAN
	DO CASE
		CASE !EMPTY(adm_finish)
			finishdate = adm_finish
		CASE !EMPTY(hdm_finish)	
			finishdate = hdm_finish
		OTHERWISE
			finishdate = CTOD("")
	ENDCASE

	REPLACE rp_finish WITH finishdate
ENDSCAN