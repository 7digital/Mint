require 'bluecloth'
require 'maruku'
require 'date'

desc 'Preview readme as html'
task :preview_readme do |task|
	maruku = Maruku.new(
		File.read('README.markdown'),
		{:title 	=> "Mint readme"}
	)

	puts maruku.to_html_document
end