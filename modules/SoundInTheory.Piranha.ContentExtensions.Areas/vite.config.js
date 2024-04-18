import { extname, relative, resolve } from 'path';
import { defineConfig, normalizePath } from 'vite';
import glob from 'glob';
import { fileURLToPath } from 'node:url';
import vue from '@vitejs/plugin-vue2';
import sass from 'sass';

var jsEntries = Object.fromEntries(
    glob.sync('resources/js/*.js').map(file => [
        // This remove `src/` as well as the file extension from each file, so e.g.
        // src/nested/foo.js becomes nested/foo
        relative('resources', file.slice(0, file.length - extname(file).length)),
        // This expands the relative paths to absolute paths, so e.g.
        // src/nested/foo becomes /project/src/nested/foo.js
        fileURLToPath(new URL(file, import.meta.url))
    ])
);

var cssEntries = Object.fromEntries(
    glob.sync('resources/scss/*.scss').map(file => [
        // This remove `src/` as well as the file extension from each file, so e.g.
        // src/nested/foo.js becomes nested/foo
        relative('resources', file.slice(0, file.length - extname(file).length)).replace("scss", "css"),
        // This expands the relative paths to absolute paths, so e.g.
        // src/nested/foo becomes /project/src/nested/foo.js
        fileURLToPath(new URL(file, import.meta.url))
    ])
);

// https://vitejs.dev/config/
export default defineConfig({
    build: {
        outDir: "./assets",
        write: true,
        emptyOutDir: true,
        manifest: true,
        minify: false,
        target: "es2015",
        cssCodeSplit: true,
        rollupOptions: {
            input: { ...jsEntries, ...cssEntries },
            output: {
                entryFileNames: '[name].js',
                assetFileNames: '[name].[ext]',
                chunkFileNames: 'js/chunks/[name]-[hash].js',
            },
            external: ['vue'],
            preserveEntrySignatures: 'strict'
        }
    },
    //publicDir: "./",
    plugins: [vue()],
})