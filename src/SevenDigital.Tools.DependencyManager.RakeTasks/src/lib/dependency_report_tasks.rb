require 'rake'
require 'rake/tasklib'
require 'chubby_rain'
include Rake

class DependencyReportTasks < TaskLib
	def initialize(options = default_options)
		options = merge_with_defaults(options)

		@executable        = options[:executable]
		@working_directory = options[:working_directory]

		yield self if block_given?

		define
	end

	def define
		define_info
		define_report
		define_conflict
		define_assert
	end

	private
	attr_reader :executable, :working_directory

	def merge_with_defaults(what)
		default_options.merge(options)
	end

	def default_options
		{
			:executable => File.expand_path(File.dirname(__FILE__) + '/../bin/chubbyrain.exe'),
			:working_directory => Dir.getwd
		}
	end

	def define_info
		desc "Configuration information for the set of tasks"
        task 'dependency:info' do
            puts "Executable: #{executable}"
			puts "Working directory: #{working_directory}"
        end
	end

	def define_report
		desc "Prints a dependency report for <assembly_name>"
        task 'dependency:report', :assembly_name do |task, args|
            puts report(complete(args.assembly_name)).text
        end
	end

	def define_conflict
		desc "Prints any dependency conflicts for <assembly_name>."
        task 'dependency:conflict', :assembly_name do |task, args|
            result = find_conflicts(complete(args.assembly_name))
	        puts result.text
        end
	end

	def define_assert
		desc "Fails with status 1 if there are any dependency conflicts"
        task 'dependency:assert', :assembly_name do |task, args|
            result = find_conflicts(complete(args.assembly_name))

            # TODO: Parse the result into conflict set and assert based on those

            if (result.text.include?('Actual Reference Version')) then
	            fail_task("There seems to be a dependency issue. #{result.text}")
	        end
        end
	end

	def report (assembly_name)
		ChubbyRain.new(executable, working_directory).run(
			Commands::REPORT,
			assembly_name
		)
	end

	def find_conflicts(assembly_name)
		ChubbyRain.new(executable, working_directory).run(
			Commands::CONFLICT,
			assembly_name
		)
	end

	def complete(assembly_name)
		return "#{assembly_name}.dll" unless assembly_name =~ /.+\.dll$/
		assembly_name
	end

	def fail_task (message = '<empty message>')
		fail("FAIL: #{message}")
	end

	class Commands
		REPORT     = 'report'
		CONFLICT    = 'conflict'
	end
end