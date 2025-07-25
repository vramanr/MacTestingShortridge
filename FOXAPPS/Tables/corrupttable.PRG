SET DEFAULT TO w:\foxapps
set tablevalidate to 0 
use shortdata!cal_data exclusive 
lcColumnName = Substr(Sys(2015),1,10) 
alter table shortdata!cal_data add column (lcColumnName) c(1) 
alter table shortdata!cal_data drop column (lcColumnName) 
USE shortdata!cal_data
set tablevalidate to 3 
