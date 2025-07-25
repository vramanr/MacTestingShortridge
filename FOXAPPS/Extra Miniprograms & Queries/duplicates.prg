set talk on
CLOSE ALL
USE w:\amount2\amount IN 1
USE BLANKID IN 2
USE duplicates IN 3
SET SAFETY OFF
custnum=""
custname=""
ordno=""
*ordnotes=""
conum=""
coname=""
PRT=""
modno=""
serno=""
matchingids=0

	select amount
	do while not eof()
		store co_id to custnum
		STORE CO_NAME TO CUSTNAME
			store order_no to ordno
			SELECT co_id, CO_NAME, order_no  ;
			FROM amount ;
			WHERE order_no==ordno ;
			AND CO_NAME==CUSTNAME ;
			and co_id==custnum ;
			and order_no <> "c" ;
			and order_no <> "C" ;
			INTO TABLE blankid.dbf
					
			select blankid
			store reccount() to matchingids
			if matchingids > 1 
				select duplicates
				Append Blank
				replace co_id with custnum
				REPLACE CO_NAME WITH CUSTNAME
				replace order_no with ordno
				endif
		select amount
	SKIP	
enddo