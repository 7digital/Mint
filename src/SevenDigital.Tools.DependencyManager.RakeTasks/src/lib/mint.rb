$:.unshift(File.dirname(__FILE__)) unless
	$:.include?(File.dirname(__FILE__)) || $:.include?(File.expand_path(File.dirname(__FILE__)))

require File.dirname(__FILE__) + '/io/process_result.rb'
require File.dirname(__FILE__) + '/chubby_rain.rb'
require File.dirname(__FILE__) + '/dependency_report_tasks.rb'