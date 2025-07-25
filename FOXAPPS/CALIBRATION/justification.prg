PROCEDURE Justification
		PARAMETERS cReading
		
		position = ATC(".", ALLTRIM(cReading))
		negposition = ATC("-", ALLTRIM(cReading))
		length = LEN(ALLTRIM(cReading))
		decimal_position = length - position
		left_digits = position - 1

		DO CASE
		CASE decimal_position = 4 AND position <> 0
			newReading =  PADL(ALLTRIM(cReading),10," ")

		CASE decimal_position = 3 AND position <> 0
			newReading = PADL(ALLTRIM(cReading)+ " ", 10, " ")

		CASE decimal_position = 2 AND position <> 0
			IF left_digits > 2
				newReading = PADL(ALLTRIM(cReading)+ "    ", 10, " ")
			ELSE
				IF left_digits = 1 OR (negposition > 0 AND left_digits = 2)
					newReading = PADL(ALLTRIM(cReading)+ "  ", 10, " ")		
				ELSE
					newReading = PADL(ALLTRIM(cReading)+ "   ", 10, " ")	
				ENDIF		
			ENDIF	 

		CASE decimal_position = 1 AND position <> 0
			newReading = PADL(ALLTRIM(cReading)+ "    ", 10, " ") 
			
		CASE position = 0
			IF length = 3
				newReading = PADL(ALLTRIM(cReading)+ "      ", 10, " ") 
			ELSE
				newReading = PADL(ALLTRIM(cReading)+ "     ", 10, " ")
			ENDIF		

		OTHERWISE
			newreading = ALLTRIM(cReading)		
		ENDCASE

		RETURN newReading
ENDPROC


PROCEDURE SetPointDecimal
	PARAMETERS 	cReading, cSetPoint, cMode, nDeviation, cType, cCalType
	
	LOCAL nDecimal
	LOCAL ARRAY aTolerances[1,2]
	
	IF ATC(".", cSetPoint) = 0 
		nDecimal = 0
	ELSE
		nDecimal = LEN(TRIM(cSetPoint))- ATC(".", cSetPoint)
	ENDIF	
	
	IF cType = 'ALLOW'	
		SELECT percent, constant FROM shortdata!tolerances WHERE mode = cMode AND cal_type = cCalType INTO ARRAY atolerances
		nDeviation = cReading * atolerances[1,1] + atolerances[1,2]
	ENDIF	

	newReading = justification(TRIM(STR(nDeviation, LEN(TRIM(cSetPoint)), nDecimal)))	
	
	DO CASE cType
	CASE  cType = 'ALLOW' 			
    	GOTO RECNO("CAL_DATA")
		REPLACE allow_dev WITH newReading		
	CASE cType = 'ACTUAL'	
		GOTO RECNO("CAL_DATA")
		REPLACE actual_dev WITH newReading	
	CASE cType = "PERCENT"
		GOTO RECNO("CAL_DATA")
		REPLACE percnt_dev WITH newReading
	ENDCASE	
	
ENDPROC	
	


PROCEDURE PadZero
		PARAMETERS cReading, cSetPoint

		cReading = ALLTRIM(cReading)

		IF ATC(".", cSetPoint) = 0
			RETURN ALLTRIM(cReading)
		ENDIF	
		
		nSetPointDecimal = LEN(ALLTRIM(cSetPoint))- ATC(".", ALLTRIM(cSetPoint))
		nPosition = ATC(".", ALLTRIM(cReading))
		nLength = LEN(ALLTRIM(cReading))
		nReadingDecimal = LEN(TRIM(cReading)) - ATC(".", TRIM(cReading))
	
		IF nPosition = 1
			cReading = "0"+ cReading
		ENDIF	
		
		IF nPosition = 0 AND nSetPointDecimal > 0
			DO Case			
			CASE nSetPointDecimal = 1
				cReading = ALLTRIM(cReading) + ".0"
			CASE nSetPointDecimal = 2
				cReading = ALLTRIM(cReading) + ".00"
			CASE nSetPointDecimal = 3
				cReading = ALLTRIM(cReading) + ".000"
			CASE nSetPointDecimal = 4
				cReading = ALLTRIM(cReading) + ".0000"
			ENDCASE
		ENDIF
			
		IF nLength = nPosition  AND nPosition > 0
			IF nSetPointDecimal = 0
				cReading = LEFT(cReading, LEN(cReading)- 1)
			ELSE 
				DO CASE		
				CASE nSetPointDecimal = 1
					cReading = ALLTRIM(cReading) + "0"
				CASE nSetPointDecimal= 2
					cReading = ALLTRIM(cReading) + "00"
				CASE nSetPointDecimal= 3
					cReading = ALLTRIM(cReading) + "000"
				CASE nSetPointDecimal = 4
					cReading = ALLTRIM(cReading) + "0000"
				ENDCASE
			ENDIF
		ENDIF				
		
		IF  nPosition > 0 AND nLength != nPosition			
			IF nSetPointDecimal - nReadingDecimal > 0 	
				DO CASE 								
				CASE nSetPointDecimal - nReadingDecimal = 1
					cReading = ALLTRIM(cReading) + "0"
				CASE nSetPointDecimal - nReadingDecimal = 2
					cReading = ALLTRIM(cReading) + "00"
				CASE nSetPointDecimal - nReadingDecimal = 3
					cReading = ALLTRIM(cReading) + "000"
				CASE nSetPointDecimal - nReadingDecimal = 4
					cReading = ALLTRIM(cReading) + "0000"		
				ENDCASE
			ELSE			
				cReading = ALLTRIM(LEFT(cReading, nPosition - 1)) + "." + ALLTRIM(LEFT(ALLTRIM(RIGHT(cReading, nReadingDecimal)), nSetPointDecimal))
			ENDIF
		ENDIF
				
		RETURN ALLTRIM(cReading)		
			
ENDPROC
	
	
	 	