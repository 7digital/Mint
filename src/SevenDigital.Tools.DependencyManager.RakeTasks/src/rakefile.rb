require 'rake'
require 'fileutils'
require 'lib/dependency_report_tasks'

WORKING_DIR = File.dirname(__FILE__) + '/../bin/1.2.121'

DependencyReportTasks.new(:working_directory => WORKING_DIR)