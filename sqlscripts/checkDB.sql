IF EXISTS (SELECT name FROM master.sys.databases WHERE name = 'CPOSDS')                                
                                  begin
                                  select 'exist' as message                              
                                  end
				else
				begin
				create database CPOSDS
				select 'created' as message				
				end


