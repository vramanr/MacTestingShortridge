set talk on
CLOSE ALL
set exclusive off
USE w:\foxapps2\foxapp\shrtridg\contact IN 1
USE BLANKID7 IN 2
USE dup_contact IN 3
select dup_contact
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

	select contact
	do while not eof()
		store co_id to custnum
		store last_name to last
		store frst_name to first
			SELECT co_id, last_name, frst_name  ;
			FROM contact ;
			WHERE last_name==last ;
			and frst_name==first ;
			and co_id==custnum ;
			and not delete() ;
			INTO TABLE blankid7.dbf
					
			select blankid7
			store reccount() to matchingids
			if matchingids > 1 
				select dup_contact
				Append Blank
				replace co_id with custnum
				REPLACE last_name WITH last
				replace frst_name with first
				endif
		select contact
	SKIP	
enddo

close all