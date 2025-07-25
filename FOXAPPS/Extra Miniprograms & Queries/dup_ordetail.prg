set talk on
CLOSE ALL
set exclusive off
USE w:\foxapps2\foxapp\shrtridg\ordetail IN 1
USE BLANKID5 IN 2
USE dup_ordetail IN 3
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

	select ordetail
	do while not eof()
		store co_id to custnum
		STORE CO_NAME TO CUSTNAME
		store order_no to ordno
		store part to prt
			SELECT co_id, CO_NAME, order_no, part  ;
			FROM ordetail ;
			WHERE order_no==ordno ;
			AND CO_NAME==CUSTNAME ;
			and co_id==custnum ;
			and part==prt ;
			and order_no <> "c" ;
			and order_no <> "C" ;	
			AND PART <> "CUSTOM" ;
			AND PART <> "ADM" ;
			AND PART <> "CFM" ;
			and part <> "HDM" ;		
			INTO TABLE blankid5.dbf
					
			select blankid5
			store reccount() to matchingids
			if matchingids > 1 
				select dup_ordetail
				Append Blank
				replace order_no with ordno				
				replace co_id with custnum
				REPLACE CO_NAME WITH CUSTNAME
				replace part with prt
				endif
		select ordetail
	SKIP	
enddo

close all