PARAMETERS cTable

OPEN DATABASE w:\foxapps\shortdata.dbc
DO CASE

CASE cTable = 'custom'
	
	USE (cTABLE)
	SELECT * FROM custom A WHERE custom IN ;
	(SELECT custom FROM custom B GROUP BY custom HAVING COUNT(custom) > 1 )  ;
	ORDER BY custom ;
	INTO CURSOR XXX
	
	COPY TO W:\foxapps\custom_duplicates.xls DELIMITED WITH TAB	
	MESSAGEBOX("File saved to Excel as " + FileName)
	
	
CASE cTable = 'PAY_DTL'
	USE (cTABLE)
	SELECT * FROM PAY_DTL A WHERE ORDER_NO IN ;
	(SELECT ORDER_NO FROM PAY_DTL B GROUP BY ORDER_NO HAVING COUNT(order_no) > 1) ;
	ORDER BY order_no ;
	INTO CURSOR XXX
	
	COPY TO W:\foxapps\pay_dtl_duplicates.xls DELIMITED WITH TAB
	MESSAGEBOX("File saved to Excel as " + FileName)
	
ENDCASE	

CLOSE TABLES


