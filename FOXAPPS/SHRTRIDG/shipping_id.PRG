OPEN DATABASE w:\foxapps\shortdata
USE shipping SHARED 

SCAN
	replace sp_coid WITH  'SP'+ ALLTRIM(shipping.co_id)
ENDSCAN