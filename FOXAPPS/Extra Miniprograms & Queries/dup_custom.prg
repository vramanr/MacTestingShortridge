set talk on
CLOSE ALL
set exclusive off
USE w:\foxapps2\foxapp\shrtridg\custom IN 1
USE BLANKID2 IN 2
USE dup_custom IN 3
select dup_custom
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

	select custom
	do while not eof()
		store co_id to custnum
		store order_no to ordno
		store custom to cstm
			SELECT co_id, order_no, custom  ;
			FROM custom ;
			WHERE order_no==ordno ;
			and custom==cstm ;
			and co_id==custnum ;
			and order_no <> "c" ;
			and order_no <> "C" ;
			and not delete() ;
			INTO TABLE blankid2.dbf
					
			select blankid2
			store reccount() to matchingids
			if matchingids > 1 
				select dup_custom
				Append Blank
				replace co_id with custnum
				REPLACE order_no WITH ordno
				replace custom with cstm
				endif
		select custom
	SKIP	
enddo

close all