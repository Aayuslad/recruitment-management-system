import { Button } from '@/components/ui/button';
import { useNavigate } from 'react-router-dom';

function LandingPage() {
    const navigate = useNavigate();

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
                    This is the{' '}
                    <span className="font-semibold">intro page</span> for RMS.
                    It will soon grow into a full-fledged platform to manage
                    end-to-end recruitment workflows.
                </p>

                <Button
                    size={'lg'}
                    className="bg-blue-600 text-white text-lg hover:bg-blue-700 hover:cursor-pointer transition mx-2"
                    onClick={() => navigate('/dashboard')}
                >
                    Home
                </Button>

                <Button
                    size={'lg'}
                    className="bg-blue-600 text-white text-lg hover:bg-blue-700 hover:cursor-pointer transition mx-2"
                    onClick={() => navigate('/login')}
                >
                    Login
                </Button>

                <Button
                    size={'lg'}
                    className="bg-blue-600 text-white text-lg hover:bg-blue-700 hover:cursor-pointer transition mx-2"
                    onClick={() => navigate('/register')}
                >
                    Register
                </Button>
            </main>

            <footer className="mt-12 text-sm text-gray-500">
                © {new Date().getFullYear()} RMS Project
            </footer>
        </div>
    );
}

export default LandingPage;
