SET DEFAULT TO w:\foxapps
set tablevalidate to 0 
use shortdata!cal_types exclusive 
lcColumnName = Substr(Sys(2015),1,10) 
alter table shortdata!cal_types add column (lcColumnName) c(1) 
alter table shortdata!cal_types drop column (lcColumnName) 
USE shortdata!cal_types
set tablevalidate to 3 

