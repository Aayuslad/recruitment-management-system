import { useAppStore } from '@/store';
import { Button } from './button';
import { Sun, Moon } from 'lucide-react';

export function ThemeToggleButton() {
    const { dark, toggle } = useAppStore();

    return (
        <Button
            size="icon"
            variant="ghost"
            onClick={toggle}
            className="flex items-center space-x-2 mx-3"
        >
            {dark ? <Sun size={96} /> : <Moon size={96} />}
        </Button>
    );
}
