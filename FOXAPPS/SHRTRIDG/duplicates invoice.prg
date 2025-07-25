PARAMETERS cTable

OPEN DATABASE W:\foxapps\shortdata.dbc
DO CASE

CASE cTable = 'AMOUNT'
	
	USE (cTABLE)
	SELECT * FROM model_no A WHERE ORDER_NO IN ;
	(SELECT ORDER_NO FROM model_no B GROUP BY ORDER_NO HAVING COUNT(ORDER_NO) > 1 ) AND order_no NOT in ('P', 'D', 'C', 'H', 'F')  ;
	ORDER BY order_no ;
	INTO CURSOR XXX
	
	*Browse
	
	COPY TO W:\foxapps\invoice_duplicates.xls DELIMITED WITH TAB	
	MESSAGEBOX("File saved to Excel as " + 'W:\foxapps\amount_duplicates.xls')
	
	
*CASE cTable = 'PAY_DTL'
*	USE (cTABLE)
*	SELECT * FROM PAY_DTL A WHERE ORDER_NO IN ;
	(SELECT ORDER_NO FROM PAY_DTL B GROUP BY ORDER_NO HAVING COUNT(order_no) > 1) AND order_no NOT in ('P', 'D', 'C', 'H', 'F') ;
	ORDER BY order_no ;
	INTO CURSOR XXX
	
	*Browse
	
*	COPY TO W:\foxapps\pay_dtl_duplicates.xls DELIMITED WITH TAB
*	MESSAGEBOX("File saved to Excel as " + 'W:\foxapps\pay_dtl_duplicates.xls')
	
ENDCASE	

CLOSE TABLES


