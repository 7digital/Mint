require "test/unit"
require File.dirname(__FILE__) + '/../../src/lib/chubby_rain'
require File.dirname(__FILE__) + '/../../src/lib/io/process_result'

class DependencyReportTests < Test::Unit::TestCase
	def setup

	end

	def test_that_run_throws_exception_when_executable_is_not_found_on_file_system
		instance = ChubbyRain.new(ANY_PATH_THAT_DOES_NOT_EXIST, "")
		assert_raise(ArgumentError) do
			instance.run(COMMAND_REPORT, ASS_NAME)
		end
	end

	def test_that_run_throws_exception_when_local_lib_path_is_not_found_on_file_system
		instance = ChubbyRain.new(EXE_PATH, ANY_PATH_THAT_DOES_NOT_EXIST)
		assert_raise(ArgumentError) do
			instance.run(COMMAND_REPORT, ASS_NAME)
		end
	end

	def test_that_run_throws_exception_when_assembly_name_is_not_supplied
		instance = ChubbyRain.new(EXE_PATH, WORKING_DIR)
		assert_raise(ArgumentError) do
			instance.run(COMMAND_REPORT, nil)
		end
	end

	def test_that_a_report_is_returned
		instance = ChubbyRain.new(EXE_PATH, WORKING_DIR)
		
		report = instance.run(COMMAND_REPORT, ASS_NAME)

		assert_equal(0, report.exit_status)

		assert_contains(report.text, "Count of assemblies referenced")
		assert_contains(report.text, "Incorrect Dependencies Summary")

		puts report.text
	end

	def test_that_a_report_with_exit_code_1_is_returned_when_assembly_does_not_exist
		instance = ChubbyRain.new(EXE_PATH, WORKING_DIR)
		
		report = instance.run(COMMAND_REPORT, CHUBBY_BAT_DLL)

		assert_equal(1, report.exit_status)

		assert_contains(report.text, "Could not load file or assembly")
		assert_contains(report.text, CHUBBY_BAT_DLL)
	end

	def test_that_a_report_with_exit_code_1_is_returned_when_command_not_supported
		instance = ChubbyRain.new(EXE_PATH, WORKING_DIR)

		a_command_that_is_not_supported = "nonexisting"

		report = instance.run( a_command_that_is_not_supported, CHUBBY_BAT_DLL)

		assert_equal(1, report.exit_status)

		expected_message = "The command is invalid: <#{a_command_that_is_not_supported}>"

		assert_contains(report.text, expected_message)
	end

	def test_that_it_fails_when_argument_is_missing
		instance = ChubbyRain.new(EXE_PATH, WORKING_DIR)

		missing_command = nil

		report = instance.run( missing_command, CHUBBY_BAT_DLL)

		assert_equal(1, report.exit_status)

		expected_message = 'Incorrect arguments, usage: chubbyrain command workingDirectory assemblyName'

		assert_contains(report.text, expected_message)
	end

	private

	def assert_contains(text, what)
		assert(
			text.include?(what),
			"Expected text <#{what}> not found in: <#{text}>"
		)
	end

	ANY_PATH_THAT_DOES_NOT_EXIST = 'c:\does-not-exist'
	EXE_PATH        = File.dirname(__FILE__) + '/../../../SevenDigital.Tools.DependencyManager/bin/Debug/chubbyrain.exe'
	COMMAND_REPORT  = 'report'
	WORKING_DIR     = File.dirname(__FILE__) + '/../../bin/1.2.121/'
	ASS_NAME        = 'Sevendigital.Domain.User.dll'
	CHUBBY_BAT_DLL  = 'Chubby.Bat.dll'
	
end