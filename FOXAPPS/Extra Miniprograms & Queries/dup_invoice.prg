set talk on
CLOSE ALL
set exclusive off
USE w:\foxapps2\foxapp\shrtridg\invoice IN 1
USE BLANKID6 IN 2
USE dup_invoice IN 3
SET SAFETY OFF
custnum=""
custname=""
ordno=""
conum=""
coname=""
PRT=""
modno=""
serno=""
matchingids=0

	select invoice
	do while not eof()
		store co_id to custnum
		STORE CO_NAME TO CUSTNAME
			store order_no to ordno
			SELECT co_id, CO_NAME, order_no  ;
			FROM invoice ;
			WHERE order_no==ordno ;
			AND CO_NAME==CUSTNAME ;
			and co_id==custnum ;
			and order_no <> "c" ;
			and order_no <> "C" ;
			INTO TABLE blankid6.dbf
					
			select blankid6
			store reccount() to matchingids
			if matchingids > 1 
				select dup_invoice
				Append Blank
				replace co_id with custnum
				REPLACE CO_NAME WITH CUSTNAME
				replace order_no with ordno
				endif
		select invoice
	SKIP	
enddo

close all