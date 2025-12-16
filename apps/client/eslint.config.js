import js from '@eslint/js';
import globals from 'globals';
import react from 'eslint-plugin-react';
import reactHooks from 'eslint-plugin-react-hooks';
import reactRefresh from 'eslint-plugin-react-refresh';
import tseslint from 'typescript-eslint';
import jsxA11y from 'eslint-plugin-jsx-a11y';
import prettier from 'eslint-config-prettier';
import { globalIgnores } from 'eslint/config';

export default tseslint.config([
    globalIgnores([
        'dist',
        'build',
        'node_modules',
        'generated',
        'src/components/ui',
        'src/types/generated/api.d.ts'
    ]),

    {
        files: ['**/*.{ts,tsx,js,jsx}'],

        languageOptions: {
            ecmaVersion: 'latest',
            sourceType: 'module',
            globals: globals.browser,
            parserOptions: {
                ecmaVersion: 'latest',
                sourceType: 'module',
                ecmaFeatures: { jsx: true },
            },
        },

        settings: {
            react: { version: 'detect' },
        },

        plugins: {
            react,
            'jsx-a11y': jsxA11y,
        },

        extends: [
            js.configs.recommended,
            tseslint.configs.recommended,
            reactHooks.configs['recommended-latest'],
            reactRefresh.configs.vite,
            prettier,
        ],

        rules: {
            /* React */
            'react/react-in-jsx-scope': 'off',
            'react/prop-types': 'off',

            /* TypeScript */
            '@typescript-eslint/no-explicit-any': 'warn',
            '@typescript-eslint/no-unused-vars': [
                'warn',
                { argsIgnorePattern: '^_' },
            ],

            /* General JS */
            'no-unused-vars': 'off', // handled by TS rule above
            eqeqeq: ['error', 'always'],
            quotes: ['error', 'single'],
            semi: ['error', 'always'],
            indent: ['warn', 4],
            'no-console': ['warn', { allow: ['warn', 'error'] }],
            'no-debugger': 'error',
        },
    },
]);
