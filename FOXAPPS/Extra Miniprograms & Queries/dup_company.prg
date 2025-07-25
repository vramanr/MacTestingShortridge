set talk on
CLOSE ALL
set exclusive off
USE w:\foxapps2\foxapp\shrtridg\company IN 1
USE BLANKID8 IN 2
USE dup_company IN 3
select dup_company
delete all
SET SAFETY OFF

custnum=""
custname=""
saddr1=""
saddr2=""
scity=""
sstate=""
zip=""
matchingids=0

	select company
	do while not eof()
		store co_id to custnum
		store co_name to custname
		store s_addr1 to saddr1
		store s_addr2 to saddr2
		store s_city to scity
		store s_state to sstate
		store s_zip to szip
			SELECT co_id, co_name, s_addr1, s_addr2, s_city, s_state, s_zip  ;
			FROM company ;
			WHERE co_name==custname ;
			and s_zip==szip ;
			and not delete() ;
			INTO TABLE blankid8.dbf
					
			select blankid8
			store reccount() to matchingids
			if matchingids > 1 
				select dup_company
				Append Blank
				replace co_id with custnum
				REPLACE co_name WITH custname
				replace s_addr1 with saddr1
				replace s_addr2 with saddr2
				replace s_city with scity
				replace s_state with sstate				
				replace s_zip with szip
				endif
		select company
	SKIP	
enddo

close all