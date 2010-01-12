require 'rake'
require 'fileutils'
require 'lib/sevendigital_mint_raketasks.tools.dependencymanager.raketasks'

EXE = File.expand_path(File.dirname(__FILE__) + '/../../SevenDigital.Tools.DependencyManager\\bin\\Debug\\Chubbyrain.exe')
WORKING_DIR = File.dirname(__FILE__) + '/../bin/1.2.121'

DependencyReportTasks.new(EXE, WORKING_DIR)