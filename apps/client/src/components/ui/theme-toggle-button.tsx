import { useAppStore } from '@/store';
import { Button } from './button';
import { Sun, Moon } from 'lucide-react'; // shadcn uses lucide icons

export function ThemeToggleButton() {
    const { dark, toggle } = useAppStore();

    return (
        <Button
            variant="outline"
            onClick={toggle}
            className="flex items-center space-x-2 mx-3"
        >
            {dark ? <Sun className="w-4 h-4" /> : <Moon className="w-4 h-4" />}
        </Button>
    );
}
