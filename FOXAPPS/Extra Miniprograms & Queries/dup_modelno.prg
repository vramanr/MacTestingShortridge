set talk on
CLOSE ALL
set exclusive off
USE w:\foxapps2\foxapp\shrtridg\model_no IN 1
USE BLANKID4 IN 2
USE dup_modelno IN 3
select dup_modelno
delete all
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

	select model_no
	do while not eof()
		store co_id to custnum
		STORE CO_NAME TO CUSTNAME
			store order_no to ordno
			store model_no to modno
			STORE SERIAL_NO TO SERNO
			SELECT co_id, CO_NAME, order_no, model_no, serial_no  ;
			FROM model_no ;
			WHERE order_no==ordno ;
			AND CO_NAME==CUSTNAME ;
			and co_id==custnum ;
			and model_no==modno ;
			and serial_no==serno ;
			and order_no <> "c" ;
			and order_no <> "C" ;
			and not delete() ;
			INTO TABLE blankid4.dbf
					
			select blankid4
			store reccount() to matchingids
			if matchingids > 1 
				select dup_modelno
				Append Blank
				replace co_id with custnum
				REPLACE CO_NAME WITH CUSTNAME
				replace order_no with ordno
				replace model_no with modno
				replace serial_no with serno
				endif
		select model_no
	SKIP	
enddo

close all