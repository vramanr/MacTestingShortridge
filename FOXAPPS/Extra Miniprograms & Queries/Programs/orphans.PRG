PARAMETERS cParent, cChild, cLink

OPEN DATABASE w:\foxapps\shortdata.dbc

DO CASE 

CASE cLink = 'co_id'
	USE (cParent)
	SELECT * FROM (cParent) A WHERE co_id NOT IN ;
	(SELECT co_id FROM (cChild) B GROUP BY co_id HAVING COUNT(co_id) > 0 )  ;
	ORDER BY co_id ;
	INTO CURSOR XXX

CASE cLink = 'order_no'
	USE (cParent)
	SELECT * FROM (cParent) A WHERE order_no NOT IN ;
	(SELECT order_no FROM (cChild) B GROUP BY order_no HAVING COUNT(order_no) > 0 )  ;
	ORDER BY order_no ;
	INTO CURSOR XXX

ENDCASE	

	
	
FileName = 'w:\foxapps\' + cParent+'_'+cChild + '.xls'
	
COPY TO (FileName) DELIMITED WITH TAB	
MESSAGEBOX("File saved to Excel as " + FileName)
CLOSE TABLES
	
	


