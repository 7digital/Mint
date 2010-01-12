Gem::Specification.new do |s|
	s.name      = 'sevendigital_mint_rake_tasks'
	s.version   = "1.0.2"
	s.has_rdoc  = false
	s.required_ruby_version = ">= 1.8.6"
	s.platform  = "ruby"
	s.require_paths = ['lib']
	s.required_rubygems_version = ">= 0"
	s.author    = "Ben Biddington, Matt Jackson, Goncalo Pereira (7digital)"
	s.email     = %q{dan.rough+mint@7digital.com}
	s.summary   = %q{Mint rake tasks}
	s.homepage  = %q{http://github.com/7digital/Mint}
	s.description = %q{Set of rake tasks for detecting .NET assembly conflicts.}
	s.files = [
		File.dirname(__FILE__) + '/lib/sevendigital_mint_rake_tasks.rb',
		File.dirname(__FILE__) + '/lib/chubby_rain.rb',
		File.dirname(__FILE__) + '/lib/dependency_report_tasks.rb',
		File.dirname(__FILE__) + '/lib/io/process_result.rb'
	]
	s.rubyforge_project = ''
	s.post_install_message = "Installation complete: #{s.name} v#{s.version}"
end