require "test/unit"
require File.dirname(__FILE__) + '/../../src/lib/chubby_rain'

class DependencyConflictTests < Test::Unit::TestCase
  def test_that_a_conflict_is_returned
		instance = ChubbyRain.new(EXE_PATH, WORKING_DIR)

		conflict = instance.run(COMMAND_REPORT, ASS_NAME)

		assert_equal(0, conflict.exit_status, "Expected exit status of 0. Message: #{conflict.text}")

		assert_contains(conflict.text, "Major Revision Conflict")
		assert_contains(conflict.text, "References: 			SevenDigital.B")
		assert_contains(conflict.text, "Reference Version: 		1.0.0.0")
		assert_contains(conflict.text, "Actual Reference Version: 	1.5.0.0")
  end

 def test_that_there_are_no_conflicts
		instance = ChubbyRain.new(EXE_PATH, WORKING_DIR)

		conflict = instance.run(COMMAND_REPORT, NO_CONFLICT_NAME)

		assert_equal(0, conflict.exit_status, "Expected exit status of 0. Message: #{conflict.text}")
		assert_not_contains("is referencing assembly",conflict.text)
end
private

def assert_contains(text, what)
		assert(
			text.include?(what),
			"Expected text <#{what}> not found in: <#{text}>"
		)
end

def assert_not_contains(text,what)
	  assert(
		 false ==  text.include?(what),
		  "Found <#{what}> in: <#{text}>"
	  )
end

	ANY_PATH_THAT_DOES_NOT_EXIST = 'c:\does-not-exist'
	EXE_PATH            = File.expand_path(File.dirname(__FILE__) + '/../../src/bin/chubbyrain.exe')
	COMMAND_REPORT      = 'conflict'
	WORKING_DIR         = File.expand_path(File.dirname(__FILE__) + '/SampleAssemblies/')
	ASS_NAME            = 'Sevendigital.A.dll'
  	NO_CONFLICT_NAME    = 'Sevendigital.B.dll'
	CHUBBY_BAT_DLL      = 'Chubby.Bat.dll'

end