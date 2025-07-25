set talk on
CLOSE ALL
set exclusive off
USE w:\amount2\amount IN 1
USE BLANKID IN 2
USE dup_amount IN 3 exclusive
select dup_amount
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
amnt=""
matchingids=0

	select amount
	do while not eof()
		store co_id to custnum
		STORE CO_NAME TO CUSTNAME
			store order_no to ordno
			store amount to amnt
			SELECT co_id, CO_NAME, order_no, amount  ;
			FROM amount ;
			WHERE order_no==ordno ;
			AND CO_NAME==CUSTNAME ;
			and co_id==custnum ;
			and order_no <> "c" ;
			and order_no <> "C" ;
			and not delete() ;
			INTO TABLE blankid.dbf
					
			select blankid
			store reccount() to matchingids
			if matchingids > 1 
				select dup_amount
				Append Blank
				replace co_id with custnum
				REPLACE CO_NAME WITH CUSTNAME
				replace order_no with ordno
				replace amount with amnt
				endif
		select amount
	SKIP	
enddo

close all