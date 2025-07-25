set talk on
CLOSE ALL
set exclusive off
USE w:\foxapps2\foxapp\shrtridg\serialno IN 1
USE w:\foxsave\BLANKID3 IN 2
USE w:\foxsave\dup_serialno IN 3
select dup_serialno
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

	select serialno
	do while not eof()
		store co_id to custnum
		STORE CO_NAME TO CUSTNAME
			store serial_no to serno
			SELECT co_id, CO_NAME, serial_no  ;
			FROM serialno ;
			WHERE serial_no==serno ;
			AND CO_NAME==CUSTNAME ;
			and co_id==custnum ;
			and not delete() ;
			INTO TABLE blankid3.dbf
					
			select blankid3
			store reccount() to matchingids
			if matchingids > 1 
				select dup_serialno
				Append Blank
				replace co_id with custnum
				REPLACE CO_NAME WITH CUSTNAME
				replace serial_no with serno
				endif
		select serialno
	SKIP	
enddo

close all