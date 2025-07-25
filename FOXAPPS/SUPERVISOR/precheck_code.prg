*** DELETE ***
		case choice = 2 
			 define window WORKING from 10,35 to 12,45 none ;
 			color N/R*
 			activate window WORKING
 			@ 1,0 Say "  DELETING"

 			path=""
 			use precheck in 1
 			do ordrpath.prg
 			use path+"ordrstat" in 2
 			use path+"order_de" in 3

			twoweeksago=date()-14
			select precheck
			delete for shipped < twoweeksago and shipped > CTOD("1/1/90")

			scan for deleted()
				store order_no to ordno
				** DELETE MATCHING ORDER NUMBERS IN ORDRSTAT **
				select ordrstat 
  					delete for order_no = ordno
				select order_de
					delete for order_no = ordno
					replace entry_no with 88 for order_no = ordno
				select precheck
			endscan
			deactivate window working

			select precheck
				scan for shipped > CTOD("1/1/90")
					store shipped to s_date
					store order_no to ordno
					select ordrstat
						scan for order_no = ordno
							replace ship_date with s_date
						endscan
					select precheck
				endscan

	*** PACK ***
		case choice = 3
			thistime= ""
			thistime = time()
			thistime = left(thistime, 2)
 			if thistime = "06" or thistime = "07"
		 		define window WORKING from 10,35 to 12,55 none ;
 				color N/R*
 				activate window WORKING
 				@ 1,0 Say "  PACKING"
	
				close all
				set exclusive on
				use precheck
				pack
				close all
				use pc_misc
				pack
				close all
				set exclusive off
				deactivate window working
			else
				define window badtime from 6,3 to 10,55 panel;
				color scheme 2
				activate window badtime
				@ 1,1 Say "Pack can only be done between  6:00 and 7:45"
				@ 2,5 Say ""
				wait
				deactivate window badtime
			endif

			

ENDCASE
RETURN 0