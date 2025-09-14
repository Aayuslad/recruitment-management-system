function App() {  
  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gray-50 text-gray-800">
      <header className="mb-8 text-center">
        <h1 className="text-4xl font-extrabold mb-2">
          Recruitment Management System
        </h1>
        <p className="text-lg text-gray-600">
          Streamlining hiring from job creation to candidate selection
        </p>
      </header>

      <main className="max-w-2xl text-center space-y-4">
        <p>
          This is the <span className="font-semibold">intro page</span> for RMS.
          It will soon grow into a full-fledged platform to manage
          end-to-end recruitment workflows.
        </p>
        <button className="px-6 py-2 bg-blue-600 text-white rounded-lg shadow hover:bg-blue-700 transition">
          Get Started
        </button>
      </main>

      <footer className="mt-12 text-sm text-gray-500">
        © {new Date().getFullYear()} RMS Project
      </footer>
    </div>
  );
}

export default App;
