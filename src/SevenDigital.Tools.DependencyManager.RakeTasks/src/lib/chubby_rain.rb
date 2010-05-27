require File.dirname(__FILE__) + '/io/process_result'
#
# Class that generates a dependency report for .NET assemblies
#
class ChubbyRain
	def initialize(executable, working_directory)
		@executable         = executable
		@working_directory  = working_directory
	end

	def run(command='report', assembly_name = nil)
		require_dependencies

		raise ArgumentError.new("No assembly supplied") if assembly_name.nil?

		stdout_result = %x{
			#{File.expand_path(executable)} #{command} "#{File.expand_path(working_directory)}" "#{assembly_name}"
		}

		ProcessResult.new($?.exitstatus, stdout_result)
	end

	private
	attr_reader :executable, :command, :sevendigital_lib_path, :working_directory

	def require_dependencies
		raise ArgumentError.new("Executable path does not exist: <#{executable}>") unless
			File.exists?(executable)
		raise ArgumentError.new("The working directory  does not exist: <#{working_directory}>") unless
			File.exists?(working_directory)
	end
end