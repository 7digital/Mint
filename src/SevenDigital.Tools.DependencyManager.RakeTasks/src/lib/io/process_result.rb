class ProcessResult
	attr_reader :exit_status, :text
	def initialize(exit_status, text)
		@exit_status = exit_status
		@text = text
	end
end